// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Core.Configuration;
using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.Input
{
    /// <summary>
    /// Static utility class which provides methods to help managing assets and functionalities of the new input system.
    /// </summary>
    public static class InputEditorUtils
    {
#if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// Copies the custom key bindings into the project by using the default one.
        /// </summary>
        public static void CopyCustomKeyBindingAsset()
        {
            UnityEngine.InputSystem.InputActionAsset defaultBindings = Resources.Load<UnityEngine.InputSystem.InputActionAsset>(RuntimeConfigurator.Configuration.DefaultInputActionAssetPath);

            if(AssetDatabase.IsValidFolder("Assets/MindPort")== false)
            {
                AssetDatabase.CreateFolder("Assets", "MindPort");
            }

            if (AssetDatabase.IsValidFolder("Assets/MindPort/VRBuilder") == false)
            {
                AssetDatabase.CreateFolder("Assets/MindPort", "VRBuilder");
            }

            if (AssetDatabase.IsValidFolder("Assets/MindPort/VRBuilder/Resources") == false)
            {
                AssetDatabase.CreateFolder("Assets/MindPort/VRBuilder", "Resources");
            }

            if (AssetDatabase.IsValidFolder("Assets/MindPort/VRBuilder/Resources/KeyBindings") == false)
            {
                AssetDatabase.CreateFolder("Assets/MindPort/VRBuilder/Resources", "KeyBindings");
            }

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(defaultBindings),
                $"Assets/MindPort/VRBuilder/Resources/{RuntimeConfigurator.Configuration.CustomInputActionAssetPath}.inputactions");

            AssetDatabase.Refresh();

            RuntimeConfigurator.Configuration.CurrentInputActionAsset =
                Resources.Load<UnityEngine.InputSystem.InputActionAsset>(RuntimeConfigurator.Configuration.CustomInputActionAssetPath);
        }

        /// <summary>
        /// Checks if the custom key bindings are loaded.
        /// </summary>
        public static bool UsesCustomKeyBindingAsset()
        {
            return AssetDatabase.GetAssetPath(RuntimeConfigurator.Configuration.CurrentInputActionAsset)
                .Equals("Assets/MindPort/VRBuilder/Resources" + RuntimeConfigurator.Configuration.CustomInputActionAssetPath);
        }

        /// <summary>
        /// Opens the key binding editor.
        /// </summary>
        public static void OpenKeyBindingEditor()
        {
            if (UsesCustomKeyBindingAsset() == false)
            {
                CopyCustomKeyBindingAsset();
            }
            AssetDatabase.OpenAsset(RuntimeConfigurator.Configuration.CurrentInputActionAsset);
        }
#else
        /// <summary>
        /// Copies the custom key bindings into the project by using the default one.
        /// </summary>
        public static void CopyCustomKeyBindingAsset()
        {
            Debug.LogError("Error, no implementation for the old input system");
        }

        /// <summary>
        /// Checks if the custom key bindings are loaded.
        /// </summary>
        public static bool UsesCustomKeyBindingAsset()
        {
            Debug.LogError("Error, no implementation for the old input system");
            return false;
        }

        /// <summary>
        /// Opens the key binding editor.
        /// </summary>
        public static void OpenKeyBindingEditor()
        {
            Debug.LogError("Error, no implementation for the old input system");
        }
#endif
    }
}
