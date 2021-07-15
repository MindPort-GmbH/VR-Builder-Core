using UnityEditor;

namespace VRBuilder.Editor.UI
{
    internal class BuilderSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Builder/Settings";

        public BuilderSettingsProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new BuilderSettingsProvider();
            return provider;
        }
    }
}
