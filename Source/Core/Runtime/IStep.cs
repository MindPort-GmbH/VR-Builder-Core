// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

namespace VRBuilder.Core
{
    /// <summary>
    /// Interface for a training step.
    /// </summary>
    public interface IStep : IDataOwner<IStepData>, IEntity
    {
        /// <summary>
        /// Step's metadata.
        /// </summary>
        StepMetadata StepMetadata { get; set; }
    }
}
