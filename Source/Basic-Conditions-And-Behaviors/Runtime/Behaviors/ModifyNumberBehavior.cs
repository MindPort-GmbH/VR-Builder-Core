using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// A behavior that sets a value property to a specified value.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ModifyNumberBehavior : Behavior<ModifyNumberBehavior.EntityData>
    {
        public enum Operator
        {
            Add,
            Subtract,
            Multiply,
            Divide,
        }

        /// <summary>
        /// The <see cref="SetValueBehavior{T}"/> behavior data.
        /// </summary>
        [DisplayName("Modify Value")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Modified Property")]
            public ScenePropertyReference<IValueProperty<float>> ModifiedProperty { get; set; }

            [DataMember]
            [DisplayName("Operator")]
            public Operator Operator { get; set; }

            [DataMember]
            [DisplayName("Modifier Property")]
            public ScenePropertyReference<IValueProperty<float>> ModifierProperty { get; set; }

            [DataMember]
            [DisplayName("Modifier Value")]
            public float ModifierValue { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }

            /// <inheritdoc />
            public string Name { get; set; }
        }

        private class ActivatingProcess : StageProcess<EntityData>
        {
            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
            }

            /// <inheritdoc />
            public override void End()
            {
                float modifier = Data.ModifierProperty.Value != null ? Data.ModifierProperty.Value.GetValue() : Data.ModifierValue;

                switch (Data.Operator)
                {
                    case Operator.Add:
                        Data.ModifiedProperty.Value.SetValue(Data.ModifiedProperty.Value.GetValue() + modifier);
                        break;
                    case Operator.Subtract:
                        Data.ModifiedProperty.Value.SetValue(Data.ModifiedProperty.Value.GetValue() - modifier);
                        break;
                    case Operator.Multiply:
                        Data.ModifiedProperty.Value.SetValue(Data.ModifiedProperty.Value.GetValue() * modifier);
                        break;
                    case Operator.Divide:
                        if (modifier == 0)
                        {
                            throw new System.DivideByZeroException();
                        }
                        Data.ModifiedProperty.Value.SetValue(Data.ModifiedProperty.Value.GetValue() + modifier);
                        break;
                }
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }

        public ModifyNumberBehavior() : this("", "", 0f, Operator.Add)
        {
        }

        public ModifyNumberBehavior(string modifiedPropertyName, string modifierPropertyName, float modifierValue, Operator arithmeticOperator, string name = "Modify Value")
        {
            Data.ModifiedProperty = new ScenePropertyReference<IValueProperty<float>>(modifiedPropertyName);
            Data.ModifierProperty = new ScenePropertyReference<IValueProperty<float>>(modifierPropertyName);
            Data.ModifierValue = modifierValue;
            Data.Operator = arithmeticOperator;
            Data.Name = name;
        }

        public ModifyNumberBehavior(IValueProperty<float> modifiedProperty, IValueProperty<float> modifierProperty, float value, Operator arithmeticOperator, string name = "Modify Value") : this(ProcessReferenceUtils.GetNameFrom(modifiedProperty), ProcessReferenceUtils.GetNameFrom(modifierProperty), value, arithmeticOperator, name)
        {
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
