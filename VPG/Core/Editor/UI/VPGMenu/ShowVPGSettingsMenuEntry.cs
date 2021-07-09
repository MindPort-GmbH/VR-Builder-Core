using VRBuilder.Editor.Configuration;
using UnityEditor;

namespace VRBuilder.Editor.VPGMenu
{
    internal static class ShowVPGSettingsMenuEntry
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
