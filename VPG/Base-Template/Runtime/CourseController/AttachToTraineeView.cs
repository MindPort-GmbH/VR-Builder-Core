using System;
using UnityEngine;
using UnityEngine.UI;
using VPG.Core.Configuration;

namespace VPG.BaseTemplate
{
    /// <summary>
    /// Puts the parent GameObject to the same position and rotation of the trainee camera.
    /// </summary>
    public class AttachToTraineeView : MonoBehaviour
    {
        [Tooltip("The font used in the spectator view.")]
        [SerializeField]
        protected Font font;
        
        [Tooltip("Size of the font used")]
        [SerializeField]
        protected int fontSize = 30;
        
        private GameObject trainee;

        protected void Start()
        {
            SetFont();
        }

        protected void LateUpdate()
        {
            UpdateCameraPositionAndRotation();
        }

        private void UpdateCameraPositionAndRotation()
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
        
        private void SetFont()
        {
            foreach (Text text in GetComponentsInChildren<Text>(true))
            {
                text.font = font;
                text.fontSize = fontSize;
            }
        }
    }
}