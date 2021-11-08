// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using UnityEngine;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Serialization;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// An interface for training runtime configurations. Implement it to create your own.
    /// </summary>
    [Obsolete("To be more flexible with development we switched to an abstract class as configuration base, consider using BaseRuntimeConfiguration.")]
    public interface IRuntimeConfiguration
    {
        /// <summary>
        /// SceneObjectRegistry gathers all created TrainingSceneEntities.
        /// </summary>
        ISceneObjectRegistry SceneObjectRegistry { get; }

        /// <summary>
        /// Defines the serializer which should be used to serialize training courses.
        /// </summary>
        ICourseSerializer Serializer { get; set; }

        /// <summary>
        /// Returns the mode handler for the training.
        /// </summary>
        IModeHandler Modes { get; }

        /// <summary>
        /// User scene object.
        /// </summary>
        TrainingSceneObject User { get; }

        /// <summary>
        /// Default audio source to play audio from.
        /// </summary>
        AudioSource InstructionPlayer { get; }

        /// <summary>
        /// Synchronously returns the deserialized training course from given path.
        /// </summary>
        ICourse LoadCourse(string path);
    }
}
