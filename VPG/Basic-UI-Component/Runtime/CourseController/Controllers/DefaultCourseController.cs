using System;
using System.Collections.Generic;

namespace VPG.UX
{
    /// <summary>
    /// Default course controller.
    /// </summary>
    public class DefaultCourseController : UIBaseCourseController
    {
        /// <inheritdoc />
        public override string Name { get; } = "Default";

        /// <inheritdoc />
        protected override string PrefabName { get; } = "DefaultCourseController";

        /// <inheritdoc />
        public override int Priority { get; } = 50;

        /// <inheritdoc />
        public override string CourseMenuPrefabName { get; } = "CourseControllerMenu";

        /// <inheritdoc />
        public override List<Type> GetRequiredSetupComponents()
        {
            List<Type> requiredSetupComponents = base.GetRequiredSetupComponents();
            requiredSetupComponents.Add(typeof(SpectatorController));
            return requiredSetupComponents;
        }
    }
}
