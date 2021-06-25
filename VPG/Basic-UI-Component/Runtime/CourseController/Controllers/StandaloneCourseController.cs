using System;
using System.Collections.Generic;
using VPG.Core.Internationalization;
using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Course controller for standalone devices like the Oculus Quest.
    /// </summary>
    public class StandaloneCourseController : BaseCourseController
    {
        /// <inheritdoc />
        public override string Name { get; } = "Standalone";
        
        /// <inheritdoc />
        public override int Priority { get; } = 25;
        
        /// <inheritdoc />
        public override LocalizationConfig LocalizationConfig { get; } = Resources.Load<LocalizationConfig>(LocalizationConfig.StandaloneDefaultLocalizationConfig);
        
        /// <inheritdoc />
        protected override string PrefabName { get; } = "StandaloneCourseController";
        
        /// <inheritdoc />
        public string CourseMenuPrefabName { get; } = "StandaloneCourseControllerMenu";
        
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
            List<Type> requiredComponents = base.GetRequiredSetupComponents();
            requiredComponents.Add(typeof(CourseMenuSpawner));
            return requiredComponents;
        }

        /// <inheritdoc />
        public override void HandlePostSetup(GameObject courseControllerObject)
        {
            courseControllerObject.GetComponent<CourseMenuSpawner>().SetDefaultPrefab(GetCourseMenuPrefab());
        }
    }
}