using VPG.Core.Conditions;
using VPG.BasicInteraction.Conditions;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.BasicInteraction.UI.Conditions
{
    /// <inheritdoc />
    public class TeleportMenuItem : MenuItem<ICondition>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Teleport";

        /// <inheritdoc />
        public override ICondition GetNewItem()
        {
            return new TeleportCondition();
        }
    }
}

