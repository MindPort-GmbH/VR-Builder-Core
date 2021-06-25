using System;
using System.Collections.Generic;
using VPG.Core.Internationalization;
using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Base course controller which instantiates a defined prefab.
    /// </summary>
    public abstract class BaseCourseController : ICourseController, ILocalizationProvider
    {
        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract int Priority { get; }

        /// <inheritdoc />
        public virtual LocalizationConfig LocalizationConfig { get; } = Resources.Load<LocalizationConfig>(LocalizationConfig.DefaultLocalizationConfig);

        /// <summary>
        /// Name of the course controller prefab.
        /// </summary>
        protected abstract string PrefabName { get; }

        /// <inheritdoc />
        public virtual GameObject GetCourseControllerPrefab()
        {
            if (PrefabName == null)
            {
                return null;
            }
            return Resources.Load<GameObject>($"Prefabs/{PrefabName}");
        }

        /// <inheritdoc />
        public virtual List<Type> GetRequiredSetupComponents() 
        {
            return new List<Type>();
        }

        /// <inheritdoc />
        public virtual void HandlePostSetup(GameObject courseControllerObject)
        {
            // do nothing
        }
    }
}