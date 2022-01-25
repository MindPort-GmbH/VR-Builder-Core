using VRBuilder.Core.Behaviors;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class SetTextValueMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Data/Set Text";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new SetValueBehavior<string>();
        }
    }
}
