﻿// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Core.Configuration.Modes;

namespace VRBuilder.Core
{
    /// <summary>
    /// The basic interface for all components of a training course: behaviors, conditions, transitions, and so on.
    /// Do not implement this interface directly.
    /// Use <see cref="Behaviors.Behavior"/> or <see cref="Conditions.Condition"/> abstract classes instead.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The entity's life cycle.
        /// </summary>
        ILifeCycle LifeCycle { get; }

        /// <summary>
        /// Returns a new instance of a process for the Activating <seealso cref="Stage"/>.
        /// </summary>
        IProcess GetActivatingProcess();

        /// <summary>
        /// Returns a new instance of a process for the Active <seealso cref="Stage"/>.
        /// </summary>
        IProcess GetActiveProcess();

        /// <summary>
        /// Returns a new instance of a process for the Deactivating <seealso cref="Stage"/>.
        /// </summary>
        IProcess GetDeactivatingProcess();

        /// <summary>
        /// Configures the entity according to the given <paramref name="mode"/>.
        /// </summary>
        void Configure(IMode mode);

        /// <summary>
        /// Called every frame during the Unity's update.
        /// </summary>
        void Update();
    }
}
