using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// Behavior that waits for `DelayTime` seconds before finishing its activation.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-behaviors.html#delay")]
    public class DelayBehavior : Behavior<DelayBehavior.EntityData>
    {
        /// <summary>
        /// The data class for a delay behavior.
        /// </summary>
        [DisplayName("Delay")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Delay (in seconds)")]
            public float DelayTime { get; set; }

            public Metadata Metadata { get; set; }

            public string Name { get; set; }
        }

        public DelayBehavior() : this(0)
        {
        }

        public DelayBehavior(float delayTime, string name = "Delay")
        {
            if (delayTime < 0f)
            {
                Debug.LogWarningFormat("DelayTime has to be zero or positive, but it was {0}. Setting to 0 instead.", delayTime);
                delayTime = 0f;
            }

            Data.DelayTime = delayTime;
            Data.Name = name;
        }

        private class ActivatingProcess : Process<EntityData>
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
                float timeStarted = Time.time;

                while (Time.time - timeStarted < Data.DelayTime)
                {
                    yield return null;
                }
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

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
