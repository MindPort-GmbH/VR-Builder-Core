using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.UI.Wizard
{
    internal class WelcomePage : WizardPage
    {
        public WelcomePage() : base("Welcome")
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Welcome to the VR Builder", BuilderEditorStyles.Title);
                GUILayout.Label("We want to get you started with the VR Builder as fast as possible.\nThis Wizard guides you through the process.", BuilderEditorStyles.Paragraph);
            GUILayout.EndArea();
        }
    }
}
