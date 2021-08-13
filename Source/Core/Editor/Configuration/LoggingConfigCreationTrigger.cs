// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.IO;
using VRBuilder.Core.Utils.Logging;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VRBuilder.Editor.Configuration
{
    /// <summary>
    /// Checks on editor initialization if there is a logging config. Will add one if it's missing.
    /// </summary>
    [InitializeOnLoad]
    public class LoggingConfigCreationTrigger
    {
        static LoggingConfigCreationTrigger()
        {
            LifeCycleLoggingConfig instance = Resources.Load<LifeCycleLoggingConfig>("LifeCycleLoggingConfig");
            if (instance == null)
            {
                instance = ScriptableObject.CreateInstance<LifeCycleLoggingConfig>();
                if (Directory.Exists("Assets/MindPort/VRBuilder/Resources") == false)
                {
                    Directory.CreateDirectory("Assets/MindPort/VRBuilder/Resources");
                }
                
                AssetDatabase.CreateAsset(instance, "Assets/MindPort/VRBuilder/Resources/LifeCycleLoggingConfig.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
