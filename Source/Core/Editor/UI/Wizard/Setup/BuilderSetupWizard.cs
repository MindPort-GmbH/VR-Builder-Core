using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VRBuilder.Editor.PackageManager;
using VRBuilder.Editor.XRUtils;

namespace VRBuilder.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard which guides the user through setting up a new training project,
    /// including a training course, scene and XR hardware.
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
        [MenuItem("Tools/VR Builder/Create New Course...", false, 0)]
#endif
        internal static void Show()
        {
            WizardWindow wizard = EditorWindow.CreateInstance<WizardWindow>();
            List<WizardPage> pages = new List<WizardPage>()
            {
                new WelcomePage(),
                new TrainingSceneSetupPage(),
                //new AnalyticsPage(),
                new AllAboutPage()
            };

            int xrSetupIndex = 2;
#if CREATOR_PRO
            if (CreatorPro.Account.UserAccount.IsAllowedToUsePro() == false)
            {
                pages.Insert(1, new CreatorPro.Core.CreatorLoginPage());
                xrSetupIndex++;
            }
#endif
            bool isShowingXRSetupPage = EditorReflectionUtils.AssemblyExists(XRDefaultAssemblyName);
            isShowingXRSetupPage &= EditorReflectionUtils.AssemblyExists(XRAssemblyName) == false;
            isShowingXRSetupPage &= XRLoaderHelper.GetCurrentXRConfiguration()
                .Contains(XRLoaderHelper.XRConfiguration.XRLegacy) == false;

            if (isShowingXRSetupPage)
            {
                pages.Insert(xrSetupIndex, new XRSDKSetupPage());
            }

            wizard.WizardClosing += OnWizardClosing;

            wizard.Setup("VR Builder - VR Training Setup Wizard", pages);
            wizard.ShowModalUtility();
        }

        private static void OnWizardClosing(object sender, EventArgs args)
        {
            ((WizardWindow)sender).WizardClosing -= OnWizardClosing;
            SetupFinished?.Invoke(sender, args);
        }
    }
}