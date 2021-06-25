using VPG.Core.Conditions;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Conditions
{
    /// <inheritdoc />
    public class TimeoutMenuItem : MenuItem<ICondition>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Timeout";

        /// <inheritdoc />
        public override ICondition GetNewItem()
        {
            return new TimeoutCondition();
        }
    }
}
