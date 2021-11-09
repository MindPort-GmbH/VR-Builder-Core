// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using System.IO;
using VRBuilder.Core;
using VRBuilder.Core.Serialization;
using VRBuilder.Editor.Configuration;
using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor
{
    /// <summary>
    /// A static class that handles the course assets. It lets you to save, load, delete, and import processes and provides multiple related utility methods.
    /// </summary>
    internal static class ProcessAssetManager
    {
        /// <summary>
        /// Deletes the process with <paramref name="courseName"/>.
        /// </summary>
        internal static void Delete(string courseName)
        {
            if (ProcessAssetUtils.DoesCourseAssetExist(courseName))
            {
                Directory.Delete(ProcessAssetUtils.GetCourseAssetDirectory(courseName), true);
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Imports the given <paramref name="course"/> by saving it to the proper directory. If there is a name collision, this course will be renamed.
        /// </summary>
        internal static void Import(IProcess course)
        {
            int counter = 0;
            string oldName = course.Data.Name;
            while (ProcessAssetUtils.DoesCourseAssetExist(course.Data.Name))
            {
                if (counter > 0)
                {
                    course.Data.Name = course.Data.Name.Substring(0, course.Data.Name.Length - 2);
                }

                counter++;
                course.Data.Name += " " + counter;
            }

            if (oldName != course.Data.Name)
            {
                Debug.LogWarning($"We detected a name collision while importing course \"{oldName}\". We have renamed it to \"{course.Data.Name}\" before importing.");
            }

            Save(course);
        }

        /// <summary>
        /// Imports the course from file at given file <paramref name="path"/> if the file extensions matches the <paramref name="serializer"/>.
        /// </summary>
        internal static void Import(string path, IProcessSerializer serializer)
        {
            IProcess course;

            if (Path.GetExtension(path) != $".{serializer.FileFormat}")
            {
                Debug.LogError($"The file extension of {path} does not match the expected file extension of {serializer.FileFormat} of the current serializer.");
            }

            try
            {
                byte[] file = File.ReadAllBytes(path);
                course = serializer.CourseFromByteArray(file);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.GetType().Name} occured while trying to import file '{path}' with serializer '{serializer.GetType().Name}'\n\n{e.StackTrace}");
                return;
            }

            Import(course);
        }

        /// <summary>
        /// Save the <paramref name="course"/> to the file system.
        /// </summary>
        internal static void Save(IProcess course)
        {
            try
            {
                string path = ProcessAssetUtils.GetCourseAssetPath(course.Data.Name);
                byte[] courseData = EditorConfigurator.Instance.Serializer.CourseToByteArray(course);
                WriteCourse(path, courseData);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private static void WriteCourse(string path, byte[] courseData)
        {
            FileStream stream = null;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                stream = File.Create(path);
                stream.Write(courseData, 0, courseData.Length);
                stream.Close();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Loads the course with the given <paramref name="courseName"/> from the file system and converts it into the <seealso cref="IProcess"/> instance.
        /// </summary>
        internal static IProcess Load(string courseName)
        {
            if (ProcessAssetUtils.DoesCourseAssetExist(courseName))
            {
                string courseAssetPath = ProcessAssetUtils.GetCourseAssetPath(courseName);
                byte[] courseBytes = File.ReadAllBytes(courseAssetPath);

                try
                {
                    return EditorConfigurator.Instance.Serializer.CourseFromByteArray(courseBytes);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load the course '{courseName}' from '{courseAssetPath}' because of: \n{ex.Message}");
                    Debug.LogError(ex);
                }
            }
            return null;
        }

        /// <summary>
        /// Renames the <paramref name="course"/> to the <paramref name="newName"/> and moves it to the appropriate directory. Check if you can rename before with the <seealso cref="CanRename"/> method.
        /// </summary>
        internal static void RenameCourse(IProcess course, string newName)
        {
            if (ProcessAssetUtils.CanRename(course, newName, out string errorMessage) == false)
            {
                Debug.LogError($"Course {course.Data.Name} was not renamed because:\n\n{errorMessage}");
                return;
            }

            string oldDirectory = ProcessAssetUtils.GetCourseAssetDirectory(course.Data.Name);
            string newDirectory = ProcessAssetUtils.GetCourseAssetDirectory(newName);

            Directory.Move(oldDirectory, newDirectory);
            File.Move($"{oldDirectory}.meta", $"{newDirectory}.meta");

            string newAsset = ProcessAssetUtils.GetCourseAssetPath(newName);
            string oldAsset = $"{ProcessAssetUtils.GetCourseAssetDirectory(newName)}/{course.Data.Name}.{EditorConfigurator.Instance.Serializer.FileFormat}";
            File.Move(oldAsset, newAsset);
            File.Move($"{oldAsset}.meta", $"{newAsset}.meta");
            course.Data.Name = newName;

            Save(course);
        }
    }
}
