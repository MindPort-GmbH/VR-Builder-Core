using System;
using System.Collections.Generic;
using VPG.Core.Input;
using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Base course controller which also takes care that a course menu is spawned.
    /// </summary>
    public abstract class UIBaseCourseController : BaseCourseController
    {
        /// <summary>
        /// Name of the course controller menu prefab.
        /// </summary>
        public abstract string CourseMenuPrefabName { get; }

        /// <summary>
        /// Gets a course controller menu game object.
        /// </summary>
        public virtual GameObject GetCourseMenuPrefab()
        {
            return Resources.Load<GameObject>($"Prefabs/{CourseMenuPrefabName}");
        }

        /// <inheritdoc />
        public override List<Type> GetRequiredSetupComponents()
        {
            return new List<Type> {typeof(CourseMenuSpawner), InputController.ConcreteType};
        }

        /// <inheritdoc />
        public override void HandlePostSetup(GameObject courseControllerObject)
        {
            courseControllerObject.GetComponent<CourseMenuSpawner>().SetDefaultPrefab(GetCourseMenuPrefab());
        }
    }
}