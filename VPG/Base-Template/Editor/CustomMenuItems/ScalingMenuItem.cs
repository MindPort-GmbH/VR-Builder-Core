using VPG.Core.Behaviors;
using VPG.BaseTemplate.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.BaseTemplate.UI.Behaviors
{
    public class ScalingMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "VPG/Scale Object";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new ScalingBehavior();
        }
    }
}
