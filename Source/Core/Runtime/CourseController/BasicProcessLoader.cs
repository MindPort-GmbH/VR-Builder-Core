using UnityEngine;
using VRBuilder.Core;
using VRBuilder.Core.Configuration;
using System;

namespace VRBuilder.CourseController
{
    /// <summary>
    /// Loads and starts the training course currently selected in the '[TRAINING_CONFIGURATION]' gameObject.
    /// </summary>
    public class BasicProcessLoader : MonoBehaviour
    {
       private void Start()
        {
            // Load training course from a file.
            string coursePath = RuntimeConfigurator.Instance.GetSelectedCourse();

            IProcess trainingCourse;

            // Try to load the in the [TRAINING_CONFIGURATION] selected training course.
            try
            {
                trainingCourse = RuntimeConfigurator.Configuration.LoadCourse(coursePath);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Error when loading training course. {exception.GetType().Name}, {exception.Message}\n{exception.StackTrace}", RuntimeConfigurator.Instance.gameObject);
                return;
            }

            // Initializes the training course. That will synthesize an audio for the training instructions, too.
            ProcessRunner.Initialize(trainingCourse);

            // Runs the training course.
            ProcessRunner.Run();
        }
    }
}
