﻿using System;
using System.Collections.Generic;
using VRBuilder.Core.Input;

namespace VRBuilder.UX
{
    /// <summary>
    /// Default process controller.
    /// </summary>
    public class DefaultProcessController : BaseProcessController
    {
        /// <inheritdoc />
        public override string Name { get; } = "Default";

        /// <inheritdoc />
        protected override string PrefabName { get; } = "DefaultProcessController";

        /// <inheritdoc />
        public override int Priority { get; } = 50;

        /// <inheritdoc />
        public override List<Type> GetRequiredSetupComponents()
        {
            List<Type> requiredSetupComponents = base.GetRequiredSetupComponents();
            requiredSetupComponents.Add(InputController.ConcreteType);
            requiredSetupComponents.Add(typeof(SpectatorController));
            return requiredSetupComponents;
        }
    }
}
