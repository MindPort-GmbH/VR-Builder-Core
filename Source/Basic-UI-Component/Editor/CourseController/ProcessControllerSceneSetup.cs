using VRBuilder.Unity;
using VRBuilder.UX;
using UnityEngine;

namespace VRBuilder.Editor.UX
{
    /// <summary>
    /// Will be called on OnSceneSetup to add the course controller menu.
    /// </summary>
    public class ProcessControllerSceneSetup : SceneSetup
    {
        /// <inheritdoc />
        public override int Priority { get; } = 20;
        
        /// <inheritdoc />
        public override string Key { get; } = "CourseControllerSetup";
        
        /// <inheritdoc />
        public override void Setup()
        {
            GameObject courseController = SetupPrefab("[COURSE_CONTROLLER]");
            if (courseController != null)
            {
                courseController.GetOrAddComponent<ProcessControllerSetup>().ResetToDefault();
            }
        }
    }
}