using UnityEngine;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.ProcessUtils;
using VRBuilder.Core.Properties;

namespace VRBuilder.Core.Tests.Conditions
{
    public class CompareTextConditionTests : CompareValuesConditionTests<string>
    {
        protected override ICondition CreateDefaultCondition()
        {
            return new CompareValuesCondition<string>("", "", "blah", "some text", true, true, new NotEqualToOperation<string>());
        }

        protected override IDataProperty<string> CreateValueProperty(string name, string value)
        {
            GameObject propertyObject = new GameObject(name);
            IDataProperty<string> property = propertyObject.AddComponent<TextDataProperty>();
            property.SetValue(value);
            return property;
        }
    }
}