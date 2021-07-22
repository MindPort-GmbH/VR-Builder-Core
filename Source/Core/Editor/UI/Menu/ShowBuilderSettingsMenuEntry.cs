using VRBuilder.Editor.Configuration;
using UnityEditor;

namespace VRBuilder.Editor.BuilderMenu
{
    internal static class ShowBuilderSettingsMenuEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Tools/VR Builder/Settings", false, 16)]
        public static void Show()
        {
            SettingsService.OpenProjectSettings("Project/VR Builder");
        }
    }
}
