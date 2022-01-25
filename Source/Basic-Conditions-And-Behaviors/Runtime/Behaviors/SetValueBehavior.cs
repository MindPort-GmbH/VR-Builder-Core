using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// A behavior that plays audio.
    /// </summary>
    [DataContract(IsReference = true)]
    public class SetValueBehavior<T> : Behavior<SetValueBehavior<T>.EntityData>
    {
        /// <summary>
        /// The "play audio" behavior's data.
        /// </summary>
        [DisplayName("Set Float")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Property")]
            [UsesSpecificProcessDrawer("FloatPropertyDrawer")]
            public ScenePropertyReference<IValueProperty<T>> ValueProperty { get; set; }

            [DataMember]
            [DisplayName("Value")]
            public T StoredValue { get; set; }

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
                Data.ValueProperty.Value.SetValue(Data.StoredValue);
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                 yield return null;
            }

            /// <inheritdoc />
            public override void End()
            {
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }

        public SetValueBehavior() : this("", default)
        {
        }

        public SetValueBehavior(string propertyName, T value, string name = "Set Value")
        {
            Data.ValueProperty = new ScenePropertyReference<IValueProperty<T>>(propertyName);
            Data.StoredValue = value;
        }

        public SetValueBehavior(IPathProperty property, T value, string name = "Set Value") : this(ProcessReferenceUtils.GetNameFrom(property), value, name)
        {
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
