﻿using VRBuilder.Core.Internationalization;
using UnityEditor;

namespace VRBuilder.Core.Editor.Internationalization
{
    /// <summary>
    /// Resets the active language so this behaves in editor the same as in a build version.
    /// </summary>
    [InitializeOnLoad]
    public static class LocalizationReset
    {
        static LocalizationReset()
        {
            EditorApplication.playModeStateChanged += change =>
            {
                if (change == PlayModeStateChange.ExitingPlayMode)
                {
                    LanguageSettings.Instance.ActiveLanguage = null;
                    Localization.entries.Clear();
                }
            };
        }
    }
}