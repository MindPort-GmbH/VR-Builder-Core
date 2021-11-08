using System.Linq;
using VRBuilder.Core.SceneObjects;
using UnityEngine;

namespace VRBuilder.BasicInteraction.Validation
{
    /// <summary>
    /// Checks if the training scene object attached to the given GameObject is listed as accepted trainin scene object.
    /// </summary>
    public class IsProcessSceneObjectValidation : Validator
    {
        [SerializeField]
        [Tooltip("All listed Training Scene Objects are valid to be snapped other will be rejected.")]
        private ProcessSceneObject[] acceptedTrainingSceneObjects = {};

        /// <summary>
        /// Adds a new TrainingSceneObject to the list.
        /// </summary>
        public void AddTrainingSceneObject(ProcessSceneObject target)
        {
            if (acceptedTrainingSceneObjects.Contains(target) == false)
            {
                acceptedTrainingSceneObjects = acceptedTrainingSceneObjects.Append(target).ToArray();
            }
        }

        /// <summary>
        /// Removes an existing training scene object from the list.
        /// </summary>
        public void RemoveTrainingSceneObject(ProcessSceneObject target)
        {
            if (acceptedTrainingSceneObjects.Contains(target))
            {
                acceptedTrainingSceneObjects = acceptedTrainingSceneObjects.Where((obj => obj != target)).ToArray();
            }
        }
        
        /// <inheritdoc />
        public override bool Validate(GameObject obj)
        {
            ProcessSceneObject trainingSceneObject = obj.GetComponent<ProcessSceneObject>();

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