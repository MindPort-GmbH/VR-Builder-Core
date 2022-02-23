using Newtonsoft.Json;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;

namespace VRBuilder.Core.Behaviors
{
    public enum AttachSlot
    {
        Head,
        LeftHand,
        RightHand,
    }

    public class AttachObjectBehavior : Behavior<AttachObjectBehavior.EntityData>
    {
        [DisplayName("Attach Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            public ScenePropertyReference<IAttachableProperty> Target { get; set; }

            [DataMember]
            public AttachSlot AttachSlot { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        // Handle data initialization in the constructor.
        [JsonConstructor]
        public AttachObjectBehavior() : this("", AttachSlot.Head)
        {
        }

        public AttachObjectBehavior(IAttachableProperty target, AttachSlot slot, string name = "Attach Object") : this(ProcessReferenceUtils.GetNameFrom(target), slot, name)
        {
        }

        public AttachObjectBehavior(string target, AttachSlot slot, string name = "Attach Object")
        {
            Data.Target = new ScenePropertyReference<IAttachableProperty>(target);
            Data.AttachSlot = slot;
            Data.Name = name;
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
                switch (Data.AttachSlot)
                {
                    case AttachSlot.Head:
                        Data.Target.Value.SceneObject.GameObject.transform.SetParent(RuntimeConfigurator.Configuration.LocalUser.transform);
                        break;
                    case AttachSlot.LeftHand:
                        Data.Target.Value.SceneObject.GameObject.transform.SetParent(RuntimeConfigurator.Configuration.LocalUser.LeftHand);
                        break;
                    case AttachSlot.RightHand:
                        Data.Target.Value.SceneObject.GameObject.transform.SetParent(RuntimeConfigurator.Configuration.LocalUser.RightHand);
                        break;
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