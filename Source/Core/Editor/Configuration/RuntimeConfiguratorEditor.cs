// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Utils;
using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.Configuration
{
    /// <summary>
    /// Custom editor for choosing the process configuration in the Unity game object inspector.
    /// </summary>
    [CustomEditor(typeof(RuntimeConfigurator))]
    public class RuntimeConfiguratorEditor : UnityEditor.Editor
    {
        private const string configuratorSelectedCoursePropertyName = "selectedCourseStreamingAssetsPath";

        private RuntimeConfigurator configurator;
        private SerializedProperty configuratorSelectedCourseProperty;

        private static readonly List<Type> configurationTypes;
        private static readonly string[] configurationTypeNames;

        private static List<string> processDisplayNames = new List<string> { "<none>" };

        private string defaultCoursePath;
        private static bool isDirty = true;

        static RuntimeConfiguratorEditor()
        {
#pragma warning disable 0618
            configurationTypes = ReflectionUtils.GetConcreteImplementationsOf<IRuntimeConfiguration>().Except(new[] {typeof(RuntimeConfigWrapper)}).ToList();
#pragma warning restore 0618
            configurationTypes.Sort(((type1, type2) => string.Compare(type1.Name, type2.Name, StringComparison.Ordinal)));
            configurationTypeNames = configurationTypes.Select(t => t.Name).ToArray();

            ProcessAssetPostprocessor.CourseFileStructureChanged += OnCourseFileStructureChanged;
        }

        /// <summary>
        /// True when the process list is empty or missing.
        /// </summary>
        public static bool IsProcessListEmpty()
        {
            if(isDirty)
            {
                PopulateProcessList();
            }

            return processDisplayNames.Count == 1 && processDisplayNames[0] == "<none>";
        }

        protected void OnEnable()
        {
            configurator = target as RuntimeConfigurator;

            configuratorSelectedCourseProperty = serializedObject.FindProperty(configuratorSelectedCoursePropertyName);

            defaultCoursePath = EditorConfigurator.Instance.CourseStreamingAssetsSubdirectory;

            // Create process path if not present.
            string absolutePath = Path.Combine(Application.streamingAssetsPath, defaultCoursePath);
            if (Directory.Exists(absolutePath) == false)
            {
                Directory.CreateDirectory(absolutePath);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Courses can change without recompile so we have to check for them.
            UpdateAvailableCourses();

            DrawRuntimeConfigurationDropDown();

            EditorGUI.BeginDisabledGroup(IsProcessListEmpty());
            {
                DrawCourseSelectionDropDown();
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open Course in Workflow window"))
                    {
                        GlobalEditorHandler.SetCurrentCourse(ProcessAssetUtils.GetCourseNameFromPath(configurator.GetSelectedProcess()));
                        GlobalEditorHandler.StartEditingCourse();
                    }

                    if (GUILayout.Button(new GUIContent("Show Course in Explorer...")))
                    {
                        string absolutePath = $"{new FileInfo(ProcessAssetUtils.GetCourseAssetPath(ProcessAssetUtils.GetCourseNameFromPath(configurator.GetSelectedProcess())))}";
                        EditorUtility.RevealInFinder(absolutePath);
                    }
                }
                GUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private static void PopulateProcessList()
        {
            List<string> courses = ProcessAssetUtils.GetAllCourses().ToList();

            // Create dummy entry if no files are present.
            if (courses.Any() == false)
            {
                processDisplayNames.Clear();
                processDisplayNames.Add("<none>");
                return;
            }

            processDisplayNames = courses;
            processDisplayNames.Sort();
        }

        private void DrawRuntimeConfigurationDropDown()
        {
            int index = configurationTypes.FindIndex(t =>
                t.AssemblyQualifiedName == configurator.GetRuntimeConfigurationName());
            index = EditorGUILayout.Popup("Configuration", index, configurationTypeNames);
            configurator.SetRuntimeConfigurationName(configurationTypes[index].AssemblyQualifiedName);
        }

        private void DrawCourseSelectionDropDown()
        {
            int index = 0;

            string courseName = ProcessAssetUtils.GetCourseNameFromPath(configurator.GetSelectedProcess());

            if (string.IsNullOrEmpty(courseName) == false)
            {
                index = processDisplayNames.FindIndex(courseName.Equals);
            }

            index = EditorGUILayout.Popup("Selected Process", index, processDisplayNames.ToArray());

            if (index < 0)
            {
                index = 0;
            }

            string newCourseStreamingAssetsPath = ProcessAssetUtils.GetCourseStreamingAssetPath(processDisplayNames[index]);

            if (IsProcessListEmpty() == false && configurator.GetSelectedProcess() != newCourseStreamingAssetsPath)
            {
                SetConfiguratorSelectedCourse(newCourseStreamingAssetsPath);
                GlobalEditorHandler.SetCurrentCourse(processDisplayNames[index]);
            }
        }

        private void SetConfiguratorSelectedCourse(string newPath)
        {
            configuratorSelectedCourseProperty.stringValue = newPath;
        }

        private static void OnCourseFileStructureChanged(object sender, ProcessAssetPostprocessorEventArgs args)
        {
            isDirty = true;
        }

        private void UpdateAvailableCourses()
        {
            if (isDirty == false)
            {
                return;
            }

            PopulateProcessList();

            if (string.IsNullOrEmpty(configurator.GetSelectedProcess()))
            {
                SetConfiguratorSelectedCourse(ProcessAssetUtils.GetCourseStreamingAssetPath(processDisplayNames[0]));
                GlobalEditorHandler.SetCurrentCourse(ProcessAssetUtils.GetCourseAssetPath(configurator.GetSelectedProcess()));
            }
        }
    }
}
