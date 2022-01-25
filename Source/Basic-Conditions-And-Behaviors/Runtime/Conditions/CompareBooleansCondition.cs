using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Properties;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// A condition that compares two booleans and completes when the comparison is true.
    /// </summary>
    [DataContract(IsReference = true)]
    public class CompareBooleansCondition : Condition<CompareBooleansCondition.EntityData>
    {
        public enum Operator
        {
            AND,            
            OR,
            XOR,
        }

        /// <summary>
        /// The data for this condition.
        /// </summary>
        [DisplayName("Compare Values")]
        public class EntityData : IConditionData
        {
            [DataMember]
            public bool LeftValue { get; set; }

            [DataMember]
            public ScenePropertyReference<IValueProperty<bool>> LeftValueProperty { get; set; }

            [DataMember]
            public Operator Operator { get; set; }

            [DataMember]
            public bool RightValue { get; set; }

            [DataMember]
            public ScenePropertyReference<IValueProperty<bool>> RightValueProperty { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            [DataMember]
            [HideInProcessInspector]
            public string Name { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            protected override bool CheckIfCompleted()
            {
                bool left = Data.LeftValueProperty.Value != null ? Data.LeftValueProperty.Value.GetValue() : Data.LeftValue;
                bool right = Data.RightValueProperty.Value != null ? Data.RightValueProperty.Value.GetValue() : Data.RightValue;

                switch (Data.Operator)
                {
                    case Operator.AND:
                        return left && right;
                    case Operator.OR:
                        return left || right;
                    case Operator.XOR:
                        return left ^ right;
                    default:
                        return false;
                }
            }
        }

        public CompareBooleansCondition() : this("", "", default, default, Operator.AND)
        {
        }

        public CompareBooleansCondition(IValueProperty<bool> leftProperty, IValueProperty<bool> rightProperty, bool leftValue, bool rightValue, Operator compareOperator, string name = "Compare Values") :
            this(ProcessReferenceUtils.GetNameFrom(leftProperty), ProcessReferenceUtils.GetNameFrom(rightProperty), leftValue, rightValue, compareOperator, name)
        {
        }

        public CompareBooleansCondition(string leftPropertyName, string rightPropertyName, bool leftValue, bool rightValue, Operator compareOperator, string name = "Compare Booleans")
        {
            Data.LeftValueProperty = new ScenePropertyReference<IValueProperty<bool>>(leftPropertyName);
            Data.RightValueProperty = new ScenePropertyReference<IValueProperty<bool>>(rightPropertyName);
            Data.LeftValue = leftValue;
            Data.RightValue = rightValue;
            Data.Operator = compareOperator;
            Data.Name = name;
        }

        /// <inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }
    }
}
