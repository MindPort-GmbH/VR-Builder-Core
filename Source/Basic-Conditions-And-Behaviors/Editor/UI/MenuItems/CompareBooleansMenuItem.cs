using VRBuilder.Core.Conditions;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Conditions
{
    /// <inheritdoc />
    public class CompareBooleansMenuItem : MenuItem<ICondition>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Data/Compare Booleans";

        /// <inheritdoc />
        public override ICondition GetNewItem()
        {
            return new CompareBooleansCondition();
        }
    }
}
