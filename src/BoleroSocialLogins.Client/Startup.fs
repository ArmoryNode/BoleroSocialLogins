// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace SocialLoginTest.Client

open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Bolero.Remoting.Client

module Program =

    [<EntryPoint>]
    let Main args =
        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<Main.MyApp>("#main")
        builder.Services.AddBoleroRemoting(builder.HostEnvironment) |> ignore
        builder.Build().RunAsync() |> ignore
        0
