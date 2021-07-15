using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.UI
{
    internal class BuilderPageProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Builder";

        public BuilderPageProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {

        }

        [SettingsProvider]
        public static SettingsProvider GetBuilderSettingsProvider()
        {
            SettingsProvider provider = new BuilderPageProvider();
            return provider;
        }
    }
}
