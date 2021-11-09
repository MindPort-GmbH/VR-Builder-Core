// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using System.Linq;
using UnityEditor;

namespace VRBuilder.Editor
{
    /// <summary>
    /// Monitors process files added or removed from the project.
    /// </summary>
    internal class ProcessAssetPostprocessor : AssetPostprocessor
    {
        /// <summary>
        /// Raised when a course file is added, removed or moved from the course folder.
        /// </summary>
        public static event EventHandler<ProcessAssetPostprocessorEventArgs> CourseFileStructureChanged;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (CourseFileStructureChanged != null &&
                importedAssets.Concat(deletedAssets)
                    .Concat(movedAssets)
                    .Concat(movedFromAssetPaths)
                    .Any(ProcessAssetUtils.IsValidCourseAssetPath))
            {
                CourseFileStructureChanged.Invoke(null, new ProcessAssetPostprocessorEventArgs());
            }
        }
    }
}
