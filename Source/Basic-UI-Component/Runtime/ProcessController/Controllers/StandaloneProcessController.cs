﻿using System;
using System.Collections.Generic;

namespace VRBuilder.UX
{
    /// <summary>
    /// Course controller for standalone devices like the Oculus Quest.
    /// </summary>
    public class StandaloneProcessController : BaseProcessController
    {
        /// <inheritdoc />
        public override string Name { get; } = "Standalone";
        
        /// <inheritdoc />
        public override int Priority { get; } = 25;
        
        /// <inheritdoc />
        protected override string PrefabName { get; } = "StandaloneProcessController";       

        /// <inheritdoc />
        public override List<Type> GetRequiredSetupComponents()
        {
            List<Type> requiredComponents = base.GetRequiredSetupComponents();
            return requiredComponents;
        }
    }
}