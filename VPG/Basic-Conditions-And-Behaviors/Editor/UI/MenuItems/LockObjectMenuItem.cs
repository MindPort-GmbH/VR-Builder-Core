using System;
using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    [Obsolete("Locking scene objects is obsoleted, consider using the 'Unlocked Objects' list in the Step window.")]
    public class LockObjectMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Lock Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new LockObjectBehavior();
        }
    }
}
