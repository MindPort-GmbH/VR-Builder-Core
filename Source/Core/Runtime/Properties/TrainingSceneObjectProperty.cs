using VRBuilder.Core.SceneObjects;
using UnityEngine;

namespace VRBuilder.Core.Properties
{
    [RequireComponent(typeof(TrainingSceneObject))]
    public abstract class TrainingSceneObjectProperty : MonoBehaviour, ISceneObjectProperty
    {
        private ISceneObject sceneObject;

        public ISceneObject SceneObject
        {
            get
            {
                if (sceneObject == null)
                {
                    sceneObject = GetComponent<ISceneObject>();
                }

                return sceneObject;
            }
        }

        protected virtual void OnEnable()
        {
        }
    }
}
