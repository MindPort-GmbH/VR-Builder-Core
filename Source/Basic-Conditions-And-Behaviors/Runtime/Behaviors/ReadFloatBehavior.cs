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
    /// A behavior that plays audio.
    /// </summary>
    [DataContract(IsReference = true)]
    public class ReadFloatBehavior : Behavior<ReadFloatBehavior.EntityData>
    {
        /// <summary>
        /// The "play audio" behavior's data.
        /// </summary>
        [DisplayName("Read Float")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Property")]
            public ScenePropertyReference<IValueProperty<float>> ValueProperty { get; set; }

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
                Debug.Log($"Reading float of value {Data.ValueProperty.Value.GetValue()}");
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

        public ReadFloatBehavior() : this("")
        {
        }

        public ReadFloatBehavior(string propertyName, string name = "Read Float")
        {
            Data.ValueProperty = new ScenePropertyReference<IValueProperty<float>>(propertyName);
        }

        public ReadFloatBehavior(IPathProperty property, string name = "Read Float") : this(ProcessReferenceUtils.GetNameFrom(property), name)
        {
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
