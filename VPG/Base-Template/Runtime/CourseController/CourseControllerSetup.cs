using System.IO;
using UnityEngine;

namespace VPG.BaseTemplate
{
    /// <summary>
    /// Allows to select a desired CourseController.
    /// </summary>
    public class CourseControllerSetup : MonoBehaviour
    {
        private enum CourseMode
        {
            Default = 0,
            Standalone = 1
        }
        
        [SerializeField]
        private CourseMode courseMode;
        
        [SerializeField, HideInInspector]
        private GameObject courseControllerPrefab = null;

        private GameObject currentControllerInstance = null;

        protected virtual void Start()
        {
            InstantiateSpectator();
        }

        private void InstantiateSpectator()
        {
            if (courseControllerPrefab == null)
            {
                throw new FileNotFoundException($"No course controller prefabs set." );
            }
            
            if (currentControllerInstance != null)
            {
                Destroy(currentControllerInstance);
            }
            
            currentControllerInstance = Instantiate(courseControllerPrefab);
        }
    }
}
