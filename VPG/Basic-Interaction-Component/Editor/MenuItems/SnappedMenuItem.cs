using VPG.BasicInteraction.Conditions;
using VPG.Core.Conditions;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.BasicInteraction.UI.Conditions
{
    public class SnappedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Snap Object";

        public override ICondition GetNewItem()
        {
            return new SnappedCondition();
        }
    }
}