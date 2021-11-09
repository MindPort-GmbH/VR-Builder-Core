// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using System.IO;
using VRBuilder.Core;
using VRBuilder.Core.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace VRBuilder.Editor.Setup
{
    /// <summary>
    /// Helper class to setup scenes and trainings.
    /// </summary>
    internal class SceneSetupUtils
    {
        public const string SceneDirectory = "Assets/Scenes";
        private const string SimpleExampleName = "Hello Creator - A 5-step Guide";

        /// <summary>
        /// Creates and saves a new scene with given <paramref name="sceneName"/>.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <param name="directory">Directory to save scene in.</param>
        public static void CreateNewScene(string sceneName, string directory = SceneDirectory)
        {
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, $"{directory}/{sceneName}.unity");
            EditorSceneManager.OpenScene($"{directory}/{sceneName}.unity");
        }

        /// <summary>
        /// Returns true if provided <paramref name="sceneName"/> exits in given <paramref name="directory"/>.
        /// </summary>
        public static bool SceneExists(string sceneName, string directory = SceneDirectory)
        {
            return File.Exists($"{directory}/{sceneName}.unity");
        }

        /// <summary>
        /// Sets up the current scene and creates a new training course for this scene.
        /// </summary>
        /// <param name="courseName">Name of the training course.</param>
        public static void SetupSceneAndTraining(string courseName)
        {
            ProcessSceneSetup.Run();

            string errorMessage = null;
            if (ProcessAssetUtils.DoesCourseAssetExist(courseName) || ProcessAssetUtils.CanCreate(courseName, out errorMessage))
            {
                if (ProcessAssetUtils.DoesCourseAssetExist(courseName))
                {
                     ProcessAssetManager.Load(courseName);
                }
                else
                {
                    ProcessAssetManager.Import(EntityFactory.CreateCourse(courseName));
                    AssetDatabase.Refresh();
                }

                SetCourseInCurrentScene(courseName);
            }

            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                Debug.LogError(errorMessage);
            }

            try
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        /// <summary>
        /// Sets the course with given <paramref name="courseName"/> for the current scene.
        /// </summary>
        /// <param name="courseName">Name of the course.</param>
        public static void SetCourseInCurrentScene(string courseName)
        {
            RuntimeConfigurator.Instance.SetSelectedCourse(ProcessAssetUtils.GetCourseStreamingAssetPath(courseName));
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            GlobalEditorHandler.SetCurrentCourse(courseName);
            GlobalEditorHandler.StartEditingCourse();
        }

        /// <summary>
        /// Creates and saves a new simple example scene.
        /// </summary>
        /// <remarks>The new scene is meant to be used for step by step guides.</remarks>
        public static void CreateNewSimpleExampleScene()
        {
            string courseName = SimpleExampleName;
            int counter = 1;

            while (ProcessAssetUtils.DoesCourseAssetExist(courseName) || ProcessAssetUtils.CanCreate(courseName, out string errorMessage) == false)
            {
                courseName = $"{SimpleExampleName}_{counter}";
                counter++;
            }

            CreateNewScene(courseName);

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Sphere";
            sphere.transform.position = new Vector3(0f, 0.5f, 2f);

            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Plane";
            plane.transform.localScale = new Vector3(2f, 2f, 2f);

            SetupSceneAndTraining(courseName);
        }
    }
}
