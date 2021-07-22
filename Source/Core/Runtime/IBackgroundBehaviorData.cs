﻿using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Behaviors;

namespace VRBuilder.Core
{
    /// <summary>
    /// Interface that enables non-blocking background behaviors.
    /// If the `IsBlocking` property returns false, the behavior will not hinder the completion of a step.
    /// </summary>
    public interface IBackgroundBehaviorData : IBehaviorData
    {
        /// <summary>
        /// If true, the behavior prevents the completion of a step until it is completed itself.
        /// If false, the behavior does not hinder the completion of a step.
        /// </summary>
        [DataMember]
        [HideInTrainingInspector]
        bool IsBlocking { get; set; }
    }
}