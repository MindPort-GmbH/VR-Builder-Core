using VPG.BasicInteraction.Conditions;
using VPG.Core.Conditions;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.BasicInteraction.UI.Conditions
{
    public class TouchedMenuItem : MenuItem<ICondition>
    {
        public override string DisplayedName { get; } = "Touch Object";

        public override ICondition GetNewItem()
        {
            return new TouchedCondition();
        }
    }
}
