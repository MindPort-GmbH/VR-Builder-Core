using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.UI
{
    internal class VPGPageProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Builder";

        public VPGPageProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider GetVPGSettingsProvider()
        {
            SettingsProvider provider = new VPGPageProvider();
            return provider;
        }
    }
}
