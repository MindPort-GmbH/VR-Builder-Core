using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Conditions;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class ReadFloatMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Data/Read Float";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new ReadFloatBehavior();
        }
    }
}
