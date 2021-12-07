using UnityEngine;
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
        private bool installXRInteractionComponent = true;

        public InteractionComponentPage() : base("Interaction Component")
        {
        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);

            GUILayout.Label("Choose Interaction Component", BuilderEditorStyles.Title);

            GUILayout.Label("Missing Interaction Component", BuilderEditorStyles.Header);

            GUILayout.Label("The default XR Interaction Component has not been found in the project. If you have installed a different interaction component or plan to do so," +
                "feel free to unckeck the checkbox. Otherwise, press next.", BuilderEditorStyles.Paragraph);

            installXRInteractionComponent = GUILayout.Toggle(installXRInteractionComponent, "Install XR Interaction Component", BuilderEditorStyles.Toggle);

            GUILayout.EndArea();
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
