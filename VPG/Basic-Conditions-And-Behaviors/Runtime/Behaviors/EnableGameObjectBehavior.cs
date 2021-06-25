using System.Runtime.Serialization;
using VPG.Core.Attributes;
using VPG.Core.SceneObjects;
using VPG.Core.Utils;

namespace VPG.Core.Behaviors
{
    /// <summary>
    /// Enables gameObject of target ISceneObject.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-behaviors.html#enable-object")]
    public class EnableGameObjectBehavior : Behavior<EnableGameObjectBehavior.EntityData>
    {
        /// <summary>
        /// "Enable game object" behavior's data.
        /// </summary>
        [DisplayName("Enable Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// The object to enable.
            /// </summary>
            [DataMember]
            [DisplayName("Object")]
            public SceneObjectReference Target { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }

            [DataMember]
            [DisplayName("Disable Object after step is complete")]
            public bool DisableOnDeactivating { get; set; }

            /// <inheritdoc />
            public string Name { get; set; }
        }

        private class ActivatingProcess : InstantProcess<EntityData>
        {
            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                Data.Target.Value.GameObject.SetActive(true);
            }
        }
        
        private class DeactivatingProcess : InstantProcess<EntityData>
        {
            public DeactivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                if (Data.DisableOnDeactivating)
                {
                    Data.Target.Value.GameObject.SetActive(false);
                }
            }
        }

        public EnableGameObjectBehavior() : this("")
        {
        }

        /// <param name="targetObject">Object to enable.</param>
        public EnableGameObjectBehavior(ISceneObject targetObject) : this(TrainingReferenceUtils.GetNameFrom(targetObject))
        {
        }

        /// <param name="targetObject">Name of the object to enable.</param>
        public EnableGameObjectBehavior(string targetObject, string name = "Enable Object")
        {
            Data.Target = new SceneObjectReference(targetObject);
            Data.Name = name;
        }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        public override IProcess GetDeactivatingProcess()
        {
            return new DeactivatingProcess(Data);
        }
    }
}
