using System;
using VPG.Core.Configuration;
using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Spectator camera which sets its viewpoint to the one of the trainee.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class SpectatorCamera : MonoBehaviour
    {
        private GameObject trainee;

        protected virtual void Start()
        {
            trainee = RuntimeConfigurator.Configuration.Trainee.GameObject;
        }

        protected virtual void Update()
        {
            UpdateCameraPositionAndRotation();
        }

        /// <summary>
        /// Sets the position and rotation of the spectator camera to the one of the trainee.
        /// </summary>
        protected virtual void UpdateCameraPositionAndRotation()
        {
            if (trainee == null)
            {
                try
                {
                    trainee = RuntimeConfigurator.Configuration.Trainee.GameObject;
                }
                catch (NullReferenceException)
                {
                    return;
                }
            }

            transform.SetPositionAndRotation(trainee.transform.position, trainee.transform.rotation);
        }
    }
}
