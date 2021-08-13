// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Core.Runtime.Properties;
using UnityEngine;

namespace VRBuilder.Core.Audio
{
    /// <summary>
    /// This class provides audio data in form of an AudioClip. Which also might not be loaded at the time needed,
    /// check first if there can be one provided.
    /// </summary>
    public interface IAudioData : ICanBeEmpty
    {
        /// <summary>
        /// Determs if the AudioSource has an AudioClip which can be played.
        /// </summary>
        bool HasAudioClip { get; }

        /// <summary>
        /// The AudioClip of this source, can be null. Best check first with HasAudio.
        /// </summary>
        AudioClip AudioClip { get; }
    }
}
