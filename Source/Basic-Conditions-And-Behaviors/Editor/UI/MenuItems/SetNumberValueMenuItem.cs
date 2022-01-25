using VRBuilder.Core.Behaviors;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class SetNumberValueMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Data/Set Number";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new SetValueBehavior<float>();
        }
    }
}
