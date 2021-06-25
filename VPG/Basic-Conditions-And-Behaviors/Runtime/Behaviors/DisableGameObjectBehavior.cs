using System.Runtime.Serialization;
using VPG.Core.Attributes;
using VPG.Core.SceneObjects;
using VPG.Core.Utils;

namespace VPG.Core.Behaviors
{
    /// <summary>
    /// Disables gameObject of target ISceneObject.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-behaviors.html#disable-object")]
    public class DisableGameObjectBehavior : Behavior<DisableGameObjectBehavior.EntityData>
    {
        /// <summary>
        /// "Disable game object" behavior's data.
        /// </summary>
        [DisplayName("Disable Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Object to disable.
            /// </summary>
            [DataMember]
            [DisplayName("Object")]
            public SceneObjectReference Target { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }

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
                Data.Target.Value.GameObject.SetActive(false);
            }
        }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
        
        public DisableGameObjectBehavior() : this("")
        {
        }

        /// <param name="targetObject">scene object to disable.</param>
        public DisableGameObjectBehavior(ISceneObject targetObject) : this(TrainingReferenceUtils.GetNameFrom(targetObject))
        {
        }

        /// <param name="targetObject">Unique name of target scene object.</param>
        public DisableGameObjectBehavior(string targetObject, string name = "Disable Object")
        {
            Data.Target = new SceneObjectReference(targetObject);
            Data.Name = name;
        }
    }
}
