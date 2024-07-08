// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace SocialLoginTest.Server

open System
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Authentication.Google
open Microsoft.AspNetCore.Authentication.MicrosoftAccount
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Bolero.Remoting.Server
open Bolero.Server
open Bolero.Templating.Server
open Serilog
open SocialLoginTest.Server.Services
open SocialLoginTest.Server.Constants
open SocialLoginTest.Server.Extensions

type Startup(configuration: IConfiguration) =
    
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc() |> ignore
        services.AddServerSideBlazor() |> ignore
        services.AddBoleroRemoting<AccountService>() |> ignore
        services.AddSingleton<IIdentityService, InMemoryIdentityService>() |> ignore
        services.AddAuthentication(fun options ->
            options.DefaultScheme <- CookieAuthenticationDefaults.AuthenticationScheme
        )
            .AddCookie(fun options ->
                options.SlidingExpiration <- true
                options.ExpireTimeSpan <- TimeSpan.FromDays(30)
            )
            .AddGoogle(fun options ->
                options.ClientId <- configuration.["Authentication:Google:ClientId"]
                options.ClientSecret <- configuration.["Authentication:Google:ClientSecret"]
                options.SaveTokens <- true
            )
            .AddMicrosoftAccount(fun options ->
                options.ClientId <- configuration.["Authentication:Microsoft:ClientId"]
                options.ClientSecret <- configuration.["Authentication:Microsoft:ClientSecret"]
                options.SaveTokens <- true
            )
        |> ignore
        services
            .AddBoleroHost()
#if DEBUG
            .AddHotReload(templateDir = __SOURCE_DIRECTORY__ + "/../BoleroSocialLogins.Client")
#endif
        |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseWebAssemblyDebugging()

        app
            .UseAuthentication()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthorization()
            .UseBlazorFrameworkFiles()
            .UseEndpoints(fun endpoints ->
#if DEBUG
                endpoints.UseHotReload()
#endif
                endpoints.MapGet("/account/login/{provider}", fun context ->
                    async {
                        let properties = AuthenticationProperties(RedirectUri = "/auth-redirect")
                        let authProvider = context.GetRouteValue "provider"
                        
                        match authProvider with
                        | Ok provider ->
                            match provider with
                            | AuthProviders.Google -> do! context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties)
                            | AuthProviders.Microsoft -> do! context.ChallengeAsync(MicrosoftAccountDefaults.AuthenticationScheme, properties)
                            | _ -> context.Response.Redirect("/")
                        | Error _ -> context.Response.Redirect("/")
                    } |> Async.AsTask
                ) |> ignore
                
                endpoints.MapGet("/account/logout", fun context ->
                    async {
                        do! context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
                        context.Response.Redirect("/")
                    } |> Async.AsTask
                ) |> ignore
                
                endpoints.MapBoleroRemoting() |> ignore
                endpoints.MapBlazorHub() |> ignore
                endpoints.MapFallbackToBolero(Index.page) |> ignore)
        |> ignore

module Program =

    [<EntryPoint>]
    let main args =
        Log.Logger <- LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()
        
        WebHost
            .CreateDefaultBuilder(args)
            .UseStaticWebAssets()
            .UseStartup<Startup>()
            .Build()
            .Run()
        0
