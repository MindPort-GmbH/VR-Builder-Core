using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class MoveObjectMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Move Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new MoveObjectBehavior();
        }
    }
}
