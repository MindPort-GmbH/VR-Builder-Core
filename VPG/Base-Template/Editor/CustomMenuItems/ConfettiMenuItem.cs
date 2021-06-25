using VPG.Core.Behaviors;
using VPG.BaseTemplate.Behaviors;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.BaseTemplate.UI.Behaviors
{
    public class ConfettiMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "VPG/Spawn Confetti";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new ConfettiBehavior();
        }
    }
}
