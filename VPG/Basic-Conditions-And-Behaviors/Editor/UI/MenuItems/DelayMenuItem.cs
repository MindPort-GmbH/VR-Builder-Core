using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class DelayMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Delay";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new DelayBehavior();
        }
    }
}
