using System;
using System.Collections.Generic;
using VRBuilder.Core.Internationalization;
using UnityEngine;

namespace VRBuilder.UX
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
        public override List<Type> GetRequiredSetupComponents()
        {
            List<Type> requiredComponents = base.GetRequiredSetupComponents();
            return requiredComponents;
        }
    }
}