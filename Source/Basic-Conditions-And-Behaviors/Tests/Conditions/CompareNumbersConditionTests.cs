using UnityEngine;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.ProcessUtils;
using VRBuilder.Core.Properties;

namespace VRBuilder.Core.Tests.Conditions
{
    public class CompareNumbersConditionTests : CompareValuesConditionTests<float>
    {
        protected override ICondition CreateDefaultCondition()
        {
            return new CompareValuesCondition<float>("", "", 5f, -6.3f, true, true, new GreaterThanOperation<float>());
        }

        protected override IDataProperty<float> CreateValueProperty(string name, float value)
        {
            GameObject propertyObject = new GameObject(name);
            IDataProperty<float> property = propertyObject.AddComponent<NumberDataProperty>();
            property.SetValue(value);
            return property;
        }
    }
}