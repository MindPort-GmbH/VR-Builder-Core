﻿using System;
using System.Linq;
using UnityEditor;
using VRBuilder.Core.Utils;
using VRBuilder.TextToSpeech;

namespace VRBuilder.Editor.TextToSpeech.UI
{
    /// <summary>
    /// This class draws list of <see cref="ITextToSpeechProvider"/> in <see cref="textToSpeechConfiguration"/>.
    /// </summary>
    [CustomEditor(typeof(TextToSpeechConfiguration))]
    public class TextToSpeechConfigurationEditor : UnityEditor.Editor
    {
        private TextToSpeechConfiguration textToSpeechConfiguration;
        private string[] providers = { "Empty" };
        private int providersIndex = 0;
        private int lastProviderSelectedIndex = 0;

        private void OnEnable()
        {
            textToSpeechConfiguration = (TextToSpeechConfiguration)target;
            providers = ReflectionUtils.GetConcreteImplementationsOf<ITextToSpeechProvider>().ToList().Where(type => type != typeof(FileTextToSpeechProvider)).Select(type => type.Name).ToArray();
            lastProviderSelectedIndex = providersIndex = string.IsNullOrEmpty(textToSpeechConfiguration.Provider) ? Array.IndexOf(providers, nameof(MicrosoftSapiTextToSpeechProvider)) : Array.IndexOf(providers, textToSpeechConfiguration.Provider);
            textToSpeechConfiguration.Provider = providers[providersIndex];
        }

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            providersIndex = EditorGUILayout.Popup("Provider", providersIndex, providers);
            
            if (providersIndex != lastProviderSelectedIndex)
            {
                lastProviderSelectedIndex = providersIndex;
                textToSpeechConfiguration.Provider = providers[providersIndex];
            }
        }
    }
}