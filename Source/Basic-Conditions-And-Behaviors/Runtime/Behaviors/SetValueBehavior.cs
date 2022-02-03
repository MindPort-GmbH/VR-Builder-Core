using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// A behavior that sets a data property to a specified value.
    /// </summary>
    [DataContract(IsReference = true)]
    public class SetValueBehavior<T> : Behavior<SetValueBehavior<T>.EntityData>
    {
        /// <summary>
        /// The <see cref="SetValueBehavior{T}"/> behavior data.
        /// </summary>
        [DisplayName("Set Value")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Data Property")]
            public ScenePropertyReference<IDataProperty<T>> DataProperty { get; set; }

            [DataMember]
            [DisplayName("Value")]
            public T NewValue { get; set; }

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
                Data.DataProperty.Value.SetValue(Data.NewValue);
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }

        public SetValueBehavior() : this("", default)
        {
        }

        public SetValueBehavior(string name) : this ("", default, name)
        {
        }

        public SetValueBehavior(string propertyName, T value, string name = "Set Value")
        {
            Data.DataProperty = new ScenePropertyReference<IDataProperty<T>>(propertyName);
            Data.NewValue = value;
        }

        public SetValueBehavior(IDataProperty<T> property, T value, string name = "Set Value") : this(ProcessReferenceUtils.GetNameFrom(property), value, name)
        {
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
