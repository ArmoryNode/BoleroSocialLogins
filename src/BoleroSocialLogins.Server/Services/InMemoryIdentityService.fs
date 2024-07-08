// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace SocialLoginTest.Server.Services

open System
open System.Collections.Generic

type InMemoryIdentityService() =
    let identityMapping = Dictionary<string, Guid>()
    
    interface IIdentityService with
        member this.GetOrCreateUserId subject =
            async {
                match identityMapping.TryGetValue subject with
                | (true, guid) -> return guid
                | (false, _) ->
                    let newId = Guid.NewGuid()
                    identityMapping.[subject] <- newId
                    return newId
            }