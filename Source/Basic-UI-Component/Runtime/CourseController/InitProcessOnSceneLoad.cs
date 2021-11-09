using System;
using VRBuilder.Core;
using VRBuilder.Core.Configuration;
using UnityEngine;

namespace VRBuilder.UX
{
    /// <summary>
    /// Initializes the <see cref="ProcessRunner"/> with the current selected course on scene start.
    /// </summary>
    public class InitProcessOnSceneLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            InitTraining();
        }

        private void InitTraining()
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
        }
    }
}