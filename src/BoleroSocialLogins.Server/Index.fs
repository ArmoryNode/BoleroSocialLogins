// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

module SocialLoginTest.Server.Index

open Bolero
open Bolero.Html
open Bolero.Server.Html
open SocialLoginTest

let page = doctypeHtml {
    head {
        meta { attr.charset "UTF-8" }
        meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
        title { "Social Login Test" }
        ``base`` { attr.href "/" }
        link { attr.rel "stylesheet"; attr.href "SocialLoginTest.Client.styles.css" }
        link { attr.rel "manifest"; attr.href "manifest.json" }
        link { attr.rel "apple-touch-icon"; attr.sizes "512x512"; attr.href "icon-512.png" }
    }
    body {
        div { attr.id "main"; comp<Client.Main.MyApp> }
        boleroScript
        script { rawHtml "navigator.serviceWorker.register('service-worker.js');" }
    }
}
