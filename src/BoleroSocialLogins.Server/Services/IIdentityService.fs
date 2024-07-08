// Copyright 2024 ArmoryNode
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace SocialLoginTest.Server.Services

open System

type IIdentityService =
    abstract GetOrCreateUserId: subject: string -> Async<Guid>