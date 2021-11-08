// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using UnityEditor;

namespace VRBuilder.Editor.BuilderMenu
{
    internal static class SetupSceneEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Tools/VR Builder/Setup Training Scene", false, 2)]
        public static void SetupScene()
        {
            ProcessSceneSetup.Run();
        }
    }
}
