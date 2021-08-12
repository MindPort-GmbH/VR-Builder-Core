// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

//namespace VRBuilder.Core.Editor
//{
//    /// <summary>
//    /// Checks if the version of the VR Builder was updated and sends an event.
//    /// </summary>
//    [InitializeOnLoad]
//    internal static class VersionCheckerEvent
//    {
//        private const string unknownVersionString = "unknown";

//        static VersionCheckerEvent()
//        {
//            CheckVersion();
//        }

//        private async static void CheckVersion()
//        {
//            string coreVersion = await EditorUtils.GetCoreVersionAsync();

//            if (Application.isBatchMode)
//            {
//                return;
//            }

//            BuilderProjectSettings settings = BuilderProjectSettings.Load();
//            if (settings == null || string.IsNullOrEmpty(settings.ProjectBuilderVersion))
//            {
//                return;
//            }

//            if (settings.ProjectBuilderVersion == unknownVersionString || coreVersion == unknownVersionString)
//            {
//                return;
//            }

//            if (settings.ProjectBuilderVersion != coreVersion)
//            {
//                IAnalyticsTracker tracker = AnalyticsUtils.CreateTracker();
//                tracker.Send(new AnalyticsEvent() { Category = "creator", Action = "updated", Label = coreVersion });
//                settings.ProjectBuilderVersion = coreVersion;
//                settings.Save();
//            }
//        }
//    }
//}
