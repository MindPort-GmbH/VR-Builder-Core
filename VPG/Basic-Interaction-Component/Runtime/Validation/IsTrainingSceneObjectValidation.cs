using System.Linq;
using VPG.Core.SceneObjects;
using UnityEngine;

namespace VPG.BasicInteraction.Validation
{
    /// <summary>
    /// Checks if the training scene object attached to the given GameObject is listed as accepted trainin scene object.
    /// </summary>
    public class IsTrainingSceneObjectValidation : Validator
    {
        [SerializeField]
        [Tooltip("All listed Training Scene Objects are valid to be snapped other will be rejected.")]
        private TrainingSceneObject[] acceptedTrainingSceneObjects = {};

        /// <summary>
        /// Adds a new TrainingSceneObject to the list.
        /// </summary>
        public void AddTrainingSceneObject(TrainingSceneObject target)
        {
            if (acceptedTrainingSceneObjects.Contains(target) == false)
            {
                acceptedTrainingSceneObjects = acceptedTrainingSceneObjects.Append(target).ToArray();
            }
        }

        /// <summary>
        /// Removes an existing training scene object from the list.
        /// </summary>
        public void RemoveTrainingSceneObject(TrainingSceneObject target)
        {
            if (acceptedTrainingSceneObjects.Contains(target))
            {
                acceptedTrainingSceneObjects = acceptedTrainingSceneObjects.Where((obj => obj != target)).ToArray();
            }
        }
        
        /// <inheritdoc />
        public override bool Validate(GameObject obj)
        {
            TrainingSceneObject trainingSceneObject = obj.GetComponent<TrainingSceneObject>();

            if (trainingSceneObject == null)
            {
                return false;
            }
            
            if (acceptedTrainingSceneObjects.Length == 0)
            {
                return true;
            }
            
            return acceptedTrainingSceneObjects.Contains(trainingSceneObject);
        }
    }
}