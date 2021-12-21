using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Utils;
using VRBuilder.Editor.PackageManager;

namespace VRBuilder.Editor.UI.Wizard
{
    /// <summary>
    /// Wizard page which prompts the user to download the XR Interaction Component
    /// </summary>
    internal class InteractionComponentPage : WizardPage
    {
        private const string xrInteractionComponentPackage = "co.mindport.builder.xrinteraction";

        [SerializeField]
        private bool installXRInteractionComponent = false;

        public InteractionComponentPage() : base("Interaction Component")
        {
        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);

            GUILayout.Label("Choose Interaction Component", BuilderEditorStyles.Title);

            if (ReflectionUtils.GetConcreteImplementationsOf<IInteractionComponentConfiguration>().Count() == 0)
            {
                HandleMissingInteractionComponent();
            }
            else
            {
                HandleMultipleInteractionComponents();
            }

            GUILayout.EndArea();
        }

        private void HandleMissingInteractionComponent()
        {
            installXRInteractionComponent = true;
            GUILayout.Label("Missing Interaction Component", BuilderEditorStyles.Header);

            GUILayout.Label("No interaction component has been found in the project. You can install the default XR Interaction component if you desire so.", BuilderEditorStyles.Paragraph);

            GUILayout.Space(16);

            if (GUILayout.Toggle(installXRInteractionComponent, "Install default XR Interaction Component.", BuilderEditorStyles.RadioButton))
            {
                installXRInteractionComponent = true;
            }

            if (GUILayout.Toggle(!installXRInteractionComponent, "Skip for now. I will install a different interaction component.", BuilderEditorStyles.RadioButton))
            {
                installXRInteractionComponent = false;
            }
        }

        private void HandleMultipleInteractionComponents()
        {
            GUILayout.Label("Multiple Interaction Components", BuilderEditorStyles.Header);

            GUILayout.Label("The following interaction components have been found in the project.", BuilderEditorStyles.Paragraph);
            GUILayout.Space(16);

            IEnumerable<Type> interactionComponents = ReflectionUtils.GetConcreteImplementationsOf<IInteractionComponentConfiguration>();

            foreach(Type type in interactionComponents)
            {
                IInteractionComponentConfiguration configuration = ReflectionUtils.CreateInstanceOfType(type) as IInteractionComponentConfiguration;
                GUILayout.Label("- " + configuration.DisplayName, BuilderEditorStyles.Paragraph);
            }

            GUILayout.Space(16);
            GUILayout.Label("More than one interaction component may cause issues, please ensure only one is present.", BuilderEditorStyles.Paragraph);
        }

        public override void Apply()
        {
            base.Apply();

            if (installXRInteractionComponent)
            {
                PackageOperationsManager.LoadPackage(xrInteractionComponentPackage);
            }
        }
    }
}
