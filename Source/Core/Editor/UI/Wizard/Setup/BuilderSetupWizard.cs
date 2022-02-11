// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VRBuilder.Editor.PackageManager;
using VRBuilder.Editor.XRUtils;
using VRBuilder.Core.Utils;
using VRBuilder.Core.Configuration;

namespace VRBuilder.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard which guides the user through setting up a new project,
    /// including a process, scene and XR hardware.
    /// </summary>
    ///
#if UNITY_2019_4_OR_NEWER && !UNITY_EDITOR_OSX
    [InitializeOnLoad]
#endif
    public static class BuilderSetupWizard
    {
        /// <summary>
        /// Will be called when the VR Builder Setup wizard is closed.
        /// </summary>
        public static event EventHandler<EventArgs> SetupFinished;

        private const string XRDefaultAssemblyName = "VRBuilder.XRInteraction";
        private const string XRAssemblyName = "Unity.XR.Management";
        static BuilderSetupWizard()
        {
            if (Application.isBatchMode == false)
            {
                DependencyManager.OnPostProcess += OnDependenciesRetrieved;
            }
        }

        private static void OnDependenciesRetrieved(object sender, DependencyManager.DependenciesEnabledEventArgs e)
        {
            BuilderProjectSettings settings = BuilderProjectSettings.Load();

            if (settings.IsFirstTimeStarted)
            {
                settings.IsFirstTimeStarted = false;
                settings.Save();
                Show();
            }

            DependencyManager.OnPostProcess -= OnDependenciesRetrieved;
        }

#if UNITY_2019_4_OR_NEWER && !UNITY_EDITOR_OSX
        [MenuItem("Tools/VR Builder/New Process Wizard...", false, 0)]
#endif
        internal static void Show()
        {
            WizardWindow wizard = EditorWindow.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new ProcessSceneSetupPage(),
                new AllAboutPage()
            };            

            int xrSetupIndex = 2;
            int interactionComponentSetupIndex = 1;
            bool isShowingInteractionComponentPage = ReflectionUtils.GetConcreteImplementationsOf<IInteractionComponentConfiguration>().Count() != 1;

            bool isShowingXRSetupPage = isShowingInteractionComponentPage == false && IsXRInteractionComponent();
            isShowingXRSetupPage &= EditorReflectionUtils.AssemblyExists(XRAssemblyName) == false;
            isShowingXRSetupPage &= XRLoaderHelper.GetCurrentXRConfiguration()
                .Contains(XRLoaderHelper.XRConfiguration.XRLegacy) == false;

            if (isShowingXRSetupPage)
            {
                pages.Insert(xrSetupIndex, new XRSDKSetupPage());
            }

            if(isShowingInteractionComponentPage)
            {
                pages.Insert(interactionComponentSetupIndex, new InteractionComponentPage());
            }

            wizard.WizardClosing += OnWizardClosing;

            wizard.Setup("VR Builder - VR Process Setup Wizard", pages);
            wizard.ShowModalUtility();
        }

        private static bool IsXRInteractionComponent()
        {
            Type interactionComponentType = ReflectionUtils.GetConcreteImplementationsOf<IInteractionComponentConfiguration>().First();
            IInteractionComponentConfiguration interactionComponentConfiguration = ReflectionUtils.CreateInstanceOfType(interactionComponentType) as IInteractionComponentConfiguration;
            return interactionComponentConfiguration.IsXRInteractionComponent;
        }

        private static void OnWizardClosing(object sender, EventArgs args)
        {
            ((WizardWindow)sender).WizardClosing -= OnWizardClosing;
            SetupFinished?.Invoke(sender, args);
        }
    }
}
