using UnityEditor;

namespace VRBuilder.Editor.UI
{
    internal class VPGSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Builder/Settings";

        public VPGSettingsProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new VPGSettingsProvider();
            return provider;
        }
    }
}
