using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Conditions;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class SetFloatMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Data/Set Float";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new SetFloatBehavior();
        }
    }
}
