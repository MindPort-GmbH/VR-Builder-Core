﻿// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.Collections;
using VRBuilder.Core;
using VRBuilder.Core.Behaviors;

namespace VRBuilder.Tests.Utils.Mocks
{
    /// <summary>
    /// Helper behavior for testing that allows explicitly triggering finishing Activation / Deactivation
    /// </summary>
    public class EndlessBehaviorMock : Behavior<EndlessBehaviorMock.EntityData>
    {
        public class EntityData : IBackgroundBehaviorData
        {
            public Metadata Metadata { get; set; }
            public string Name { get; set; }
            public bool IsBlocking { get; set; }
        }

        private class LoopProcess : IProcess
        {
            public void Start()
            {
            }

            public IEnumerator Update()
            {
                int endlessLoopProtection = 1000000;
                while (endlessLoopProtection > 0)
                {
                    endlessLoopProtection++;
                    yield return null;
                }
            }

            public void End()
            {
            }

            public void FastForward()
            {
            }
        }

        public override IProcess GetActivatingProcess()
        {
            return new LoopProcess();
        }

        public override IProcess GetDeactivatingProcess()
        {
            return new LoopProcess();
        }

        public EndlessBehaviorMock(bool isBlocking = true)
        {
            Data.IsBlocking = isBlocking;
        }
    }
}
