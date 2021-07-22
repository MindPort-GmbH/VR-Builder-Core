using VRBuilder.Editor.Configuration;
using UnityEditor;

namespace VRBuilder.Editor.BuilderMenu
{
    internal static class SetupSceneEntry
    {
        /// <summary>
        /// Setup the current unity scene to be a functioning training scene.
        /// </summary>
        [MenuItem("Tools/VR Builder/Setup Training Scene", false, 16)]
        public static void SetupScene()
        {
            TrainingSceneSetup.Run();
        }
    }
}
