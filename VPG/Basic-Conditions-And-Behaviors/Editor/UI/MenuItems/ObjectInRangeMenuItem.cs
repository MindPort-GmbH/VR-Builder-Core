using VPG.Core.Conditions;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Conditions
{
    /// <inheritdoc />
    public class ObjectInRangeMenuItem : MenuItem<ICondition>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Object Nearby";

        /// <inheritdoc />
        public override ICondition GetNewItem()
        {
            return new ObjectInRangeCondition();
        }
    }
}
