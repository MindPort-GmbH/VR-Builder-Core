// Copyright (c) 2021 MindPort GmbH
// Licensed under the Apache License, Version 2.0

using UnityEditor;
using VRBuilder.Core.Configuration;
using VRBuilder.Editor.Configuration;

namespace VRBuilder.Editor.BuilderMenu
{
    internal static class OpenCourseMenuEntry
    {
        /// <summary>
        /// Open the Workflow Editor window.
        /// </summary>
        [MenuItem("Tools/VR Builder/Open Workflow Editor", false, 2)]
        [MenuItem("Window/VR Builder/Workflow Editor", false, 100)]
        private static void OpenWorkflowEditor()
        {
            GlobalEditorHandler.SetCurrentCourse(CourseAssetUtils.GetCourseNameFromPath(RuntimeConfigurator.Instance.GetSelectedCourse()));
            GlobalEditorHandler.StartEditingCourse();
        }

        [MenuItem("Tools/VR Builder/Open Workflow Editor", true, 2)]
        [MenuItem("Window/VR Builder/Workflow Editor", true, 100)]
        private static bool ValidateOpenWorkflowEditor()
        {
            if (RuntimeConfigurator.Exists == false)
            {
                return false;
            }

            if (RuntimeConfiguratorEditor.IsCourseListEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
