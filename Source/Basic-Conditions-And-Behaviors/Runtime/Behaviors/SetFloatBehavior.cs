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
    public class SetFloatBehavior : Behavior<SetFloatBehavior.EntityData>
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
            public ScenePropertyReference<IValueProperty<float>> ValueProperty { get; set; }

            [DataMember]
            [DisplayName("Value")]
            public float FloatValue { get; set; }

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
                Data.ValueProperty.Value.SetValue(Data.FloatValue);
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

        public SetFloatBehavior() : this("", 0f)
        {
        }

        public SetFloatBehavior(string propertyName, float value, string name = "Set Float")
        {
            Data.ValueProperty = new ScenePropertyReference<IValueProperty<float>>(propertyName);
            Data.FloatValue = value;
        }

        public SetFloatBehavior(IPathProperty property, float value, string name = "Set Float") : this(ProcessReferenceUtils.GetNameFrom(property), value, name)
        {
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
