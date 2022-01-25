using VRBuilder.Core.Behaviors;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class ModifyNumberMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Data/Modify Number";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new ModifyNumberBehavior();
        }
    }
}
