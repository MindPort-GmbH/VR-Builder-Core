using VRBuilder.Core.Behaviors;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    public class AttachObjectMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Utility/Attach Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new AttachObjectBehavior();
        }
    }
}
