using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Spawns a course menu in the scene.
    /// </summary>
    public class CourseMenuSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject defaultPrefab;

        [SerializeField]
        private bool useCustomPrefab;

        [SerializeField]
        private GameObject customPrefab;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            GameObject prefab;
            if (useCustomPrefab)
            {
                if (customPrefab == null)
                {
                    Debug.LogWarning("Custom prefab in CourseMenuSpawner is not set. No trainer menu will be spawned.");
                    return;
                }

                prefab = customPrefab;
            }
            else
            {
                prefab = defaultPrefab;
            }

            Instantiate(prefab);
        }

        /// <summary>
        /// Overrides the default prefab.
        /// </summary>
        /// <param name="prefab">New default prefab.</param>
        public void SetDefaultPrefab(GameObject prefab)
        {
            defaultPrefab = prefab;
        }
    }
}