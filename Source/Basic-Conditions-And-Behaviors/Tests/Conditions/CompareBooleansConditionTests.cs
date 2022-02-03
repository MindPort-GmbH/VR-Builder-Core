using UnityEngine;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.ProcessUtils;
using VRBuilder.Core.Properties;

namespace VRBuilder.Core.Tests.Conditions
{
    public class CompareBooleansConditionTests : CompareValuesConditionTests<bool>
    {
        protected override ICondition CreateDefaultCondition()
        {
            return new CompareValuesCondition<bool>("", "", true, false, true, true, new XorOperation());
        }

        protected override IDataProperty<bool> CreateValueProperty(string name, bool value)
        {
            GameObject propertyObject = new GameObject(name);
            IDataProperty<bool> property = propertyObject.AddComponent<BooleanDataProperty>();
            property.SetValue(value);
            return property;
        }
    }
}