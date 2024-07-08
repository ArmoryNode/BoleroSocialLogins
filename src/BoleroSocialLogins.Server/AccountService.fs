// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace SocialLoginTest.Server

open System
open System.Security.Claims
open SocialLoginTest
open Bolero.Remoting.Server
open SocialLoginTest.Server.Services
open SocialLoginTest.Server.Extensions

type AccountService(ctx: IRemoteContext, identityService: IIdentityService) =
    inherit RemoteHandler<Client.Main.AccountService>()
    
    override this.Handler =
        {
            getAccountInfo = fun () ->
                async {
                    let principal = ctx.HttpContext.User
                    
                    if principal.Identity.IsAuthenticated then
                        let email = principal.GetClaimValue ClaimTypes.Email |> Option.defaultValue String.Empty
                        let sub = principal.UserId |> Option.defaultValue String.Empty
                        
                        let! userId = identityService.GetOrCreateUserId sub
                        
                        return Some { name = principal.Identity.Name; email = email; id = userId }
                    else
                        return None
                }
        }

