using System;
using System.Linq;
using VPG.Core.Utils;
using UnityEngine;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using VPG.Editor.UI.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;
using VPG.Core.Behaviors;
using VPG.Core.Conditions;

namespace VPG.Editor.Configuration
{
    /// <summary>
    /// Configurator to set the training editor configuration which is used by the training creation editor tools (like Step Inspector).
    /// </summary>
    public static class EditorConfigurator
    {
        private static readonly DefaultEditorConfiguration editorConfiguration;

        public static DefaultEditorConfiguration Instance
        {
            get { return editorConfiguration; }
        }

        static EditorConfigurator()
        {
            Type[] lowestPriorityTypes = { typeof(DefaultEditorConfiguration) };
            Type[] definitions = ReflectionUtils.GetFinalImplementationsOf<IEditorConfiguration>(lowestPriorityTypes).ToArray();

            if (definitions.Except(lowestPriorityTypes).Count() > 1)
            {
                string listOfDefinitions = string.Join("', '", definitions.Select(definition => definition.FullName).ToArray());
                Debug.LogErrorFormat(
                    "There is more than one final implementation of training editor configurations in this Unity project: '{0}'."
                    + " Remove all editor configurations except for '{1}' and the one you want to use."
                    + " '{2}' was selected as current editor configuration.",
                    listOfDefinitions,
                    typeof(DefaultEditorConfiguration).FullName,
                    definitions.First().FullName
                );
            }

            IEditorConfiguration config = (IEditorConfiguration)ReflectionUtils.CreateInstanceOfType(definitions.First());
            if (config is DefaultEditorConfiguration configuration)
            {
                editorConfiguration = configuration;
            }
            else
            {
                editorConfiguration = new EditorConfigWrapper(config);
            }

            LoadAllowedMenuItems();
        }

        [DidReloadScripts]
        private static void LoadAllowedMenuItems()
        {
            if (string.IsNullOrEmpty(Instance.AllowedMenuItemsSettingsAssetPath))
            {
                Instance.AllowedMenuItemsSettings = new AllowedMenuItemsSettings();
            }
            else
            {
                Instance.AllowedMenuItemsSettings = AllowedMenuItemsSettings.Load();
            }

            ApplyConfigurationExtensions();
        }

        private static void ApplyConfigurationExtensions()
        {
            IEnumerable<Type> extensions = ReflectionUtils.GetFinalImplementationsOf<IEditorConfigurationExtension>();

            List<Type> disabledMenuItems = new List<Type>();
            List<Type> requiredMenuItems = new List<Type>();

            foreach(Type type in extensions)
            {
                IEditorConfigurationExtension extension = (IEditorConfigurationExtension)ReflectionUtils.CreateInstanceOfType(type);
                requiredMenuItems.AddRange(extension.RequiredMenuItems.Where(menuItem => requiredMenuItems.Contains(menuItem) == false));
                disabledMenuItems.AddRange(extension.DisabledMenuItems.Where(menuItem => disabledMenuItems.Contains(menuItem) == false));
            }

            int conflicts = disabledMenuItems.RemoveAll(menuItem => requiredMenuItems.Contains(menuItem));

            if (conflicts > 0)
            {
                Debug.LogWarningFormat("Conflicts in editor configuration extensions: {0} items were both required and disabled by different extensions. They have been enabled.", conflicts);
            }

            foreach (Type menuItem in disabledMenuItems)
            {
                if (menuItem.IsSubclassOf(typeof(MenuItem<IBehavior>)))
                {
                    Instance.AllowedMenuItemsSettings.SerializedBehaviorSelections[menuItem.AssemblyQualifiedName] = false;
                }

                if (menuItem.IsSubclassOf(typeof(MenuItem<ICondition>)))
                {
                    Instance.AllowedMenuItemsSettings.SerializedConditionSelections[menuItem.AssemblyQualifiedName] = false;
                }
            }

            foreach (Type menuItem in requiredMenuItems)
            {
                if (menuItem.IsSubclassOf(typeof(MenuItem<IBehavior>)))
                {
                    Instance.AllowedMenuItemsSettings.SerializedBehaviorSelections[menuItem.AssemblyQualifiedName] = true;
                }

                if (menuItem.IsSubclassOf(typeof(MenuItem<ICondition>)))
                {
                    Instance.AllowedMenuItemsSettings.SerializedConditionSelections[menuItem.AssemblyQualifiedName] = true;
                }
            }
        }
    }
}
