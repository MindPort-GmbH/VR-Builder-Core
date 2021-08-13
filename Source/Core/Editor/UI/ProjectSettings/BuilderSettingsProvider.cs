// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using UnityEditor;

namespace VRBuilder.Editor.UI
{
    internal class BuilderSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Builder/Settings";

        public BuilderSettingsProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new BuilderSettingsProvider();
            return provider;
        }
    }
}
