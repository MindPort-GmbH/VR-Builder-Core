using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class BehaviorSequenceMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Behaviors Sequence";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new BehaviorSequence();
        }
    }
}
