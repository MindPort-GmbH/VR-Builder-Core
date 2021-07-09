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
    /// VPG version used last time this was checked.
    /// </summary>
    [HideInInspector]
    public string ProjectVPGVersion = null;

    /// <summary>
    /// Loads the VR Builder settings for this Unity project from Resources.
    /// </summary>
    /// <returns>VPG Settings</returns>
    public static BuilderProjectSettings Load()
    {
        BuilderProjectSettings settings = Resources.Load<BuilderProjectSettings>("BuilderProjectSettings");
        if (settings == null)
        {
            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }
            // Create an instance
            settings = CreateInstance<BuilderProjectSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Resources/BuilderProjectSettings.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return settings;
        }
        return settings;
    }

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ProjectVPGVersion))
        {
            ProjectVPGVersion = EditorUtils.GetCoreVersion();
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
