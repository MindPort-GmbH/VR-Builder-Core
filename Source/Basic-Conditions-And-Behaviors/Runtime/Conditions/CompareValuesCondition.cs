using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Properties;
using System;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// A condition that completes when a certain amount of time has passed.
    /// </summary>
    [DataContract(IsReference = true)]
    public class CompareValuesCondition<T> : Condition<CompareValuesCondition<T>.EntityData> where T : IComparable<T>
    {
        public enum Operator
        {
            EqualTo,
            NotEqualTo,
            GreaterThan,
            LessThan,
            GreaterThanOrEqual,
            LessThanOrEqual,
        }        
        
        /// <summary>
        /// The data for timeout condition.
        /// </summary>
        [DisplayName("Compare Values")]
        public class EntityData : IConditionData
        {
            [DataMember]
            public T LeftValue { get; set; }

            [DataMember]
            public ScenePropertyReference<IValueProperty<T>> LeftValueProperty { get; set; }

            [DataMember]
            public Operator Operator { get; set; }

            [DataMember]
            public T RightValue { get; set; }

            [DataMember]
            public ScenePropertyReference<IValueProperty<T>> RightValueProperty { get; set; }

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
                T left = Data.LeftValueProperty.Value != null ? Data.LeftValueProperty.Value.GetValue() : Data.LeftValue;
                T right = Data.RightValueProperty.Value != null ? Data.RightValueProperty.Value.GetValue() : Data.RightValue;

                switch(Data.Operator)
                {
                    case Operator.EqualTo:
                        return left.CompareTo(right) == 0;
                    case Operator.NotEqualTo:
                        return left.CompareTo(right) != 0;
                    case Operator.LessThanOrEqual:
                        return left.CompareTo(right) <= 0;
                    case Operator.GreaterThanOrEqual:
                        return left.CompareTo(right) >= 0;
                    case Operator.LessThan:
                        return left.CompareTo(right) < 0;
                    case Operator.GreaterThan:
                        return left.CompareTo(right) > 0;
                    default:
                        return false;
                }
            }
        }

        public CompareValuesCondition() : this("", "", default, default, Operator.EqualTo)
        {
        }

        public CompareValuesCondition(string name) : this("", "", default, default, Operator.EqualTo, name)
        {
        }

        public CompareValuesCondition(IValueProperty<T> leftProperty, IValueProperty<T> rightProperty, T leftValue, T rightValue, Operator compareOperator, string name = "Compare Values") :
            this(ProcessReferenceUtils.GetNameFrom(leftProperty), ProcessReferenceUtils.GetNameFrom(rightProperty), leftValue, rightValue, compareOperator, name)
        { 
        }          

        public CompareValuesCondition(string leftPropertyName, string rightPropertyName, T leftValue, T rightValue, Operator compareOperator, string name = "Compare Values")
        {
            Data.LeftValueProperty = new ScenePropertyReference<IValueProperty<T>>(leftPropertyName);
            Data.RightValueProperty = new ScenePropertyReference<IValueProperty<T>>(rightPropertyName);
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
