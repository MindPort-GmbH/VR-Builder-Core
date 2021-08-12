﻿// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Core.Properties;

namespace VRBuilder.Tests.Utils.Mocks
{
    public class LockablePropertyMock : LockableProperty, ILockablePropertyMock
    {
        protected override void InternalSetLocked(bool lockState)
        {
        }
    }
}
