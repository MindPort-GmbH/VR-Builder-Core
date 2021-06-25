using VPG.Core.Conditions;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Conditions
{
    /// <inheritdoc />
    public class ObjectInColliderMenuItem : MenuItem<ICondition>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Move Object into Collider";

        /// <inheritdoc />
        public override ICondition GetNewItem()
        {
            return new ObjectInColliderCondition();
        }
    }
}
