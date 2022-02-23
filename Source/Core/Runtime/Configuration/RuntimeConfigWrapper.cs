// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using VRBuilder.Core.SceneObjects;
using UnityEngine;
using VRBuilder.Core.Properties;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// This wrapper is used for <see cref="IRuntimeConfiguration"/> configurations, which
    /// ensures that the old interface based configurations can still be used.
    /// </summary>
    [Obsolete("Helper class to ensure backwards compatibility.")]
    public class RuntimeConfigWrapper : BaseRuntimeConfiguration
    {
        /// <summary>
        /// Wrapped IRuntimeConfiguration.
        /// </summary>
        public readonly IRuntimeConfiguration Configuration;

        public RuntimeConfigWrapper(IRuntimeConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <inheritdoc />
        [Obsolete("Use LocalUser instead.")]
        public override ProcessSceneObject User => Configuration.LocalUser;

        /// <inheritdoc />
        public override UserSceneObject LocalUser => Configuration.LocalUser;

        /// <inheritdoc />
        public override AudioSource InstructionPlayer => Configuration.InstructionPlayer;

        /// <inheritdoc />
        public override ISceneObjectRegistry SceneObjectRegistry => Configuration.SceneObjectRegistry;

        /// <inheritdoc />
        public override IProcess LoadProcess(string path)
        {
            return Configuration.LoadProcess(path);
        }
    }
}
