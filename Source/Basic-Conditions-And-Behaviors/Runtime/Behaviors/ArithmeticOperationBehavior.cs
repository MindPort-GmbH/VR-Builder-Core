using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.ProcessUtils;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// A behavior that performs an operation on a <see cref="NumberDataProperty"/> and sets it to the new value.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ArithmeticOperationBehavior : Behavior<ArithmeticOperationBehavior.EntityData>
    {
        /// <summary>
        /// The <see cref="ArithmeticOperationBehavior"/> behavior data.
        /// </summary>
        [DisplayName("Arithmetic Operation")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [HideInProcessInspector]
            [UsesSpecificProcessDrawer("ValuePropertyDrawer")]
            public ScenePropertyReference<IDataProperty<float>> ModifiedProperty { get; set; }

            [DataMember]
            [HideInProcessInspector]
            public IProcessVariableOperation<float, float> Operation { get; set; }

            [DataMember]
            [HideInProcessInspector]
            public ScenePropertyReference<IDataProperty<float>> ModifierProperty { get; set; }

            [DataMember]
            [HideInProcessInspector]
            public float ModifierConst { get; set; }

            [DataMember]
            [HideInProcessInspector]
            public bool IsModifierConst { get; set; }

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
                float modifierValue = Data.IsModifierConst ? Data.ModifierConst : Data.ModifierProperty.Value.GetValue();

                Data.ModifiedProperty.Value.SetValue(Data.Operation.Execute(Data.ModifiedProperty.Value.GetValue(), modifierValue));
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }

        public ArithmeticOperationBehavior() : this("", "", 0f, true, new SumOperation())
        {
        }

        public ArithmeticOperationBehavior(string modifiedPropertyName, string modifierPropertyName, float modifierValue, bool isModifierConst, IProcessVariableOperation<float, float> operation, string name = "Arithmetic Operation")
        {
            Data.ModifiedProperty = new ScenePropertyReference<IDataProperty<float>>(modifiedPropertyName);
            Data.ModifierProperty = new ScenePropertyReference<IDataProperty<float>>(modifierPropertyName);
            Data.ModifierConst = modifierValue;           
            Data.IsModifierConst = isModifierConst;
            Data.Operation = operation;
            Data.Name = name;
        }

        public ArithmeticOperationBehavior(IDataProperty<float> modifiedProperty, IDataProperty<float> modifierProperty, float value, bool isModifierConst, IProcessVariableOperation<float, float> operation, string name = "Arithmetic Operation") : 
            this(ProcessReferenceUtils.GetNameFrom(modifiedProperty), ProcessReferenceUtils.GetNameFrom(modifierProperty), value, isModifierConst, operation, name)
        {
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
