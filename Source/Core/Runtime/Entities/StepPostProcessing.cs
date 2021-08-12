﻿// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

namespace VRBuilder.Core
{
    /// <summary>
    /// <see cref="IStep"/> implementation of <see cref="EntityPostProcessing{T}"/>.
    /// </summary>
    public class StepPostProcessing : EntityPostProcessing<IStep>
    {
        /// <inheritdoc />
        public override void Execute(IStep entity)
        {
            ITransition transition = EntityFactory.CreateTransition();
            entity.Data.Transitions.Data.Transitions.Add(transition);
        }
    }
}
