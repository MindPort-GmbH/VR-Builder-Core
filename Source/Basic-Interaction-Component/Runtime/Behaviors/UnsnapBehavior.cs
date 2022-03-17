using System.Collections;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Utils;
using VRBuilder.BasicInteraction.Properties;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core;

namespace VRBuilder.BasicInteraction.Behaviors
{
    /// <summary>
    /// This behavior unsnaps an object from a snapzone.
    /// </summary>
    [DataContract(IsReference = true)]
    public class UnsnapBehavior : Behavior<UnsnapBehavior.EntityData>
    {
        [DisplayName("Unsnap")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            [DisplayName("Object to unsnap")]
            public ScenePropertyReference<ISnappableProperty> SnappedObject { get; set; }
            
            [DataMember]
            [DisplayName("Snap zone to unsnap")]
            public ScenePropertyReference<ISnapZoneProperty> SnapZone { get; set; }

            public Metadata Metadata { get; set; }
            public string Name { get; set; }
        }

        public UnsnapBehavior() : this("", "")
        {
        }

        public UnsnapBehavior(ISnappableProperty snappedObject, ISnapZoneProperty snapZone) : this(ProcessReferenceUtils.GetNameFrom(snappedObject), ProcessReferenceUtils.GetNameFrom(snapZone))
        {
        }

        public UnsnapBehavior(string snappedObjectName, string snapZoneName)
        {
            Data.SnappedObject = new ScenePropertyReference<ISnappableProperty>(snappedObjectName);
            Data.SnapZone = new ScenePropertyReference<ISnapZoneProperty>(snapZoneName);
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
                ISnapZoneProperty snapZoneProperty = null;

                if (Data.SnapZone.Value != null && (Data.SnapZone.Value.SnappedObject == Data.SnappedObject.Value || Data.SnappedObject.Value == null))
                {
                    snapZoneProperty = Data.SnapZone.Value;
                }
                else if(Data.SnapZone.Value == null && Data.SnappedObject.Value != null && Data.SnappedObject.Value.IsSnapped)
                {
                    snapZoneProperty = Data.SnappedObject.Value.SnappedZone;
                }

                if(snapZoneProperty != null)
                {
                    ISnapZone snapZone = snapZoneProperty.SnapZoneObject.GetComponent<ISnapZone>();

                    if(snapZone != null)
                    {
                        snapZoneProperty.SetLocked(false);
                        snapZone.ForceRelease();
                        snapZoneProperty.SetLocked(true);
                    }
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
