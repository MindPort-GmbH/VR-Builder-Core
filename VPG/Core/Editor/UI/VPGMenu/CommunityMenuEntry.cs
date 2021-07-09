using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.VPGMenu
{
    internal static class CommunityMenuEntry
    {
        /// <summary>
        /// Allows to open the URL to Innoactive community.
        /// </summary>
        [MenuItem("Tools/VR Builder/Innoactive Help/Community", false, 80)]
        private static void OpenCommunityPage()
        {
            Application.OpenURL("https://innoactive.io/creator/community");
        }
    }
}
