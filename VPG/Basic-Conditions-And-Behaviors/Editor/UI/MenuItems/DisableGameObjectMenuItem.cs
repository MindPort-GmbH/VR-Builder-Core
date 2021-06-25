using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class DisableGameObjectMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Disable Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new DisableGameObjectBehavior();
        }
    }
}
