using VRBuilder.Core.Behaviors;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
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
