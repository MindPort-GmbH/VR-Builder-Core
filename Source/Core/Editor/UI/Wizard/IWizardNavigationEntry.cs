using UnityEngine;

namespace VRBuilder.Editor.UI.Wizard
{
    internal interface IWizardNavigationEntry
    {
        bool Selected { get; set; }
        void Draw(Rect window);
    }
}
