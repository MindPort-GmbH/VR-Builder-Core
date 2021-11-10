// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.IO;
using VRBuilder.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Settings for a VR Builder Unity project.
/// </summary>
public partial class BuilderProjectSettings : ScriptableObject
{
    /// <summary>
    /// Was the VR Builder imported and therefore started for the first time.
    /// </summary>
    [HideInInspector]
    public bool IsFirstTimeStarted = true;

    /// <summary>
    /// Builder version used last time this was checked.
    /// </summary>
    [HideInInspector]
    public string ProjectBuilderVersion = null;

    /// <summary>
    /// Loads the VR Builder settings for this Unity project from Resources.
    /// </summary>
    /// <returns>Settings</returns>
    public static BuilderProjectSettings Load()
    {
        BuilderProjectSettings settings = Resources.Load<BuilderProjectSettings>("BuilderProjectSettings");
        if (settings == null)
        {
            if (!Directory.Exists("Assets/MindPort/VR Builder/Resources"))
            {
                Directory.CreateDirectory("Assets/MindPort/VR Builder/Resources");
            }
            // Create an instance
            settings = CreateInstance<BuilderProjectSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/MindPort/VR Builder/Resources/BuilderProjectSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return settings;
        }
        return settings;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ProjectBuilderVersion))
        {
            ProjectBuilderVersion = EditorUtils.GetCoreVersion();
        }
    }

    /// <summary>
    /// Saves the VR Builder settings.
    /// </summary>
    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
