using VRBuilder.Core.Behaviors;
using VRBuilder.BaseTemplate.Behaviors;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.BaseTemplate.UI.Behaviors
{
    public class ScalingMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "VR Builder/Scale Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new ScalingBehavior();
        }
    }
}
