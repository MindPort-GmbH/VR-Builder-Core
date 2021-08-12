// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Editor.Configuration;
using UnityEditor;

namespace VRBuilder.Editor.BuilderMenu
{
    internal static class ShowBuilderSettingsMenuEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Tools/VR Builder/Settings", false, 16)]
        public static void Show()
        {
            SettingsService.OpenProjectSettings("Project/VR Builder");
        }
    }
}
