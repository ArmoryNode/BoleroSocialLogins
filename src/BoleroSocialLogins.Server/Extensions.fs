// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

module SocialLoginTest.Server.Extensions

open System.Security.Claims
open Microsoft.AspNetCore.Http

type HttpContext with
    member this.GetRouteValue (key: string) =
        match this.Request.RouteValues.TryGetValue(key) with
        | (true, outVar) ->
            match outVar with
            | :? string as v -> Ok v
            | _ -> Error "Failed to cast route value to string"
        | (false, _) -> Error "Route value not found"

type ClaimsPrincipal with
    member this.GetClaimValue (claimType: string) =
        this.Claims
        |> Seq.tryFind (fun c -> c.Type = claimType)
        |> Option.map _.Value
        
    member this.UserId =
        this.GetClaimValue ClaimTypes.NameIdentifier