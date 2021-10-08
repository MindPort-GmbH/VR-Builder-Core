// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Used to identify a trainee within the scene.
    /// </summary>
    public class TraineeSceneObject : TrainingSceneObject
    {
        protected new void Awake()
        {
            base.Awake();
            uniqueName = "Trainee";
        }
    }
}
