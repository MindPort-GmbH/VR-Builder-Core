using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class HighlightObjectMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Highlight Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new HighlightObjectBehavior();
        }
    }
}
