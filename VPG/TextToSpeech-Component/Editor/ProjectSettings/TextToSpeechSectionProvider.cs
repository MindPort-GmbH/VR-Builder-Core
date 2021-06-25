using System;
using VPG.TextToSpeech;
using VPG.Editor.UI;
using UnityEditor;
using UnityEngine;

namespace VPG.Editor.TextToSpeech.UI.ProjectSettings
{
    /// <summary>
    /// Provides text to speech settings.
    /// </summary>
    public class TextToSpeechSectionProvider : IProjectSettingsSection
    {
        /// <inheritdoc/>
        public string Title { get; } = "Text to Speech";
        
        /// <inheritdoc/>
        public Type TargetPageProvider { get; } = typeof(LanguageSettingsProvider);
        
        /// <inheritdoc/>
        public int Priority { get; } = 0;
        
        /// <inheritdoc/>
        public void OnGUI(string searchContext)
        {
            GUILayout.Label("Configuration for your Text to Speech provider.", VPGEditorStyles.ApplyPadding(VPGEditorStyles.Label, 0));
        
            GUILayout.Space(8);
        
            TextToSpeechConfiguration config = TextToSpeechConfiguration.Instance;
            UnityEditor.Editor.CreateEditor(config, typeof(VPG.Editor.TextToSpeech.UI.TextToSpeechConfigurationEditor)).OnInspectorGUI();

            GUILayout.Space(8);
        
            VPGGUILayout.DrawLink("Need Help? Visit our documentation", "https://developers.innoactive.de/documentation/creator/latest/articles/developer/12-text-to-speech.html", 0);
        }
        
        ~TextToSpeechSectionProvider()
        {
            if (EditorUtility.IsDirty(TextToSpeechConfiguration.Instance))
            {
                TextToSpeechConfiguration.Instance.Save();
            }
        }
    }
}