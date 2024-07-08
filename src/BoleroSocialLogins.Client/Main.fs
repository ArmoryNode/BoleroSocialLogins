// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

module SocialLoginTest.Client.Main

open System
open Bolero.Remoting
open Elmish
open Bolero
open Bolero.Html
open Bolero.Templating.Client
open Microsoft.AspNetCore.Components

type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/auth-redirect">] Authenticated

type UserInfo =
    {
        name: string
        email: string
        id: Guid
    }
    
type Error =
    | SignInFailed of string

type Model =
    {
        page: Page
        userInfo: UserInfo option
        errorMessage: string option
        loaded: bool
        error: exn option
    }

let initModel =
    {
        page = Home
        userInfo = None
        error = None
        loaded = false
        errorMessage = None
    }

type Message =
    | PageChanged of Page
    | SignInWithGoogle
    | SignInWithMicrosoft
    | SignOut
    | SignedOut
    | GetAccount
    | GotAccount of Option<UserInfo>
    | Error of exn

type AccountService =
    {
        getAccountInfo: unit -> Async<Option<UserInfo>>
    }
    interface IRemoteService with
        member this.BasePath = "/account"

let update (nav: NavigationManager) service message model =
    match message with
    | PageChanged page ->
        match page with
        | Authenticated ->
            { model with page = Home }, Cmd.ofMsg GetAccount
        | _ ->
            { model with page = page }, Cmd.ofMsg GetAccount
    | SignInWithGoogle ->
        nav.NavigateTo("/account/login/google", forceLoad = true)
        model, Cmd.none
    | SignInWithMicrosoft ->
        nav.NavigateTo("/account/login/microsoft", forceLoad = true)
        model, Cmd.none
    | SignOut ->
        nav.NavigateTo("/account/logout", forceLoad = true)
        { model with loaded = false }, Cmd.ofMsg SignedOut
    | SignedOut ->
        { model with userInfo = None; page = Home }, Cmd.none
    | GetAccount ->
        // Only grab user info if it's not already present
        let cmd =
            match model.userInfo with
            | Some _ -> Cmd.none
            | None -> Cmd.OfAsync.either service.getAccountInfo () GotAccount Error
        model, cmd
    | GotAccount userInfo ->
        { model with userInfo = userInfo; loaded = true }, Cmd.none
    | Error exn ->
        { model with error = Some exn; loaded = true }, Cmd.none
        

let view model dispatch =
    div {
        cond model.loaded <| function
        | true ->
            div {
                p {
                    cond model.userInfo <| function
                    | Some user ->
                        div {
                            p { $"Hello {user.name}!" }
                            p { $"User id: {user.id}" }
                        }
                    | None -> p { $"Hello world!" }
                }
                cond model.userInfo <| function
                | Some _ -> button { on.click (fun _ -> dispatch SignOut); "Sign Out" }
                | None ->
                    div {
                        button { on.click (fun _ -> dispatch SignInWithGoogle); "Google Signin" }
                        span { attr.style "margin-left: 10px" }
                        button { on.click (fun _ -> dispatch SignInWithMicrosoft); "Microsoft Signin" }
                    }
            }
        | false ->
            div { "Loading..." }
    }

let router = Router.infer PageChanged _.page

type MyApp() =
    inherit ProgramComponent<Model, Message>()
    
    override this.Program =
        let accountService = this.Remote<AccountService>()
        
        let update = update this.NavigationManager accountService
        
        Program.mkProgram (fun _ -> initModel, Cmd.ofMsg GetAccount) update view
        |> Program.withRouter router
#if DEBUG
        |> Program.withHotReload
#endif
