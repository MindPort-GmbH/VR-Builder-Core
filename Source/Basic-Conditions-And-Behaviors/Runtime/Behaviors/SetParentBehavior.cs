using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Behaviors
{
    // This behavior changes the parent of a game object in the scene hierarchy.
    [DataContract(IsReference = true)]
    public class SetParentBehavior : Behavior<SetParentBehavior.EntityData>
    {
        [DisplayName("Set Parent")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            // Process object to reparent.
            [DataMember]
            public SceneObjectReference Target { get; set; }

            // New parent game object.
            [DataMember]
            public SceneObjectReference Parent { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        // Handle data initialization in the constructor.
        [JsonConstructor]
        public SetParentBehavior() : this(new SceneObjectReference(), new SceneObjectReference())
        {
        }

        public SetParentBehavior(SceneObjectReference target, SceneObjectReference parent)
        {
            Data.Target = target;
            Data.Parent = parent;
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
                if (Data.Parent.Value == null)
                {
                    Data.Target.Value.GameObject.transform.SetParent(null);
                }
                else
                {
                    Data.Target.Value.GameObject.transform.SetParent(Data.Parent.Value.GameObject.transform);
                }
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
