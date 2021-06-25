using VPG.Core.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class EnableGameObjectMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Enable Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new EnableGameObjectBehavior();
        }
    }
}
