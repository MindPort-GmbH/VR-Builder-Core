using System.Collections.Generic;
using System.Runtime.Serialization;
using VPG.BasicInteraction.Properties;
using VPG.Core;
using VPG.Core.Attributes;
using VPG.Core.Conditions;
using VPG.Core.RestrictiveEnvironment;
using VPG.Core.SceneObjects;
using VPG.Core.Utils;
using VPG.Core.Validation;

namespace VPG.BasicInteraction.Conditions
{
    /// <summary>
    /// Condition which is completed when `GrabbableProperty` is grabbed.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html#grab-object")]
    public class GrabbedCondition : Condition<GrabbedCondition.EntityData>
    {
        [DisplayName("Grab Object")]
        public class EntityData : IConditionData
        {
#if CREATOR_PRO
            [CheckForCollider]
#endif
            [DataMember]
            [DisplayName("Object")]
            public ScenePropertyReference<IGrabbableProperty> GrabbableProperty { get; set; }
            
            public bool IsCompleted { get; set; }

            [DataMember]
            [HideInTrainingInspector]
            public string Name { get; set; }

            [DataMember]
            [DisplayName("Keep object grabbable after step")]
            public bool KeepUnlocked = true;

            public Metadata Metadata { get; set; }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            public override void Complete()
            {
                Data.GrabbableProperty.Value.FastForwardGrab();
            }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            protected override bool CheckIfCompleted()
            {
                return Data.GrabbableProperty.Value.IsGrabbed;
            }

            public ActiveProcess(EntityData data) : base(data)
            {
            }
        }

        public GrabbedCondition() : this("")
        {
        }

        public GrabbedCondition(IGrabbableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public GrabbedCondition(string target, string name = "Grab Object")
        {
            Data.GrabbableProperty = new ScenePropertyReference<IGrabbableProperty>(target);
            Data.Name = name;
        }
        
        public override IEnumerable<LockablePropertyData> GetLockableProperties()
        {
            IEnumerable<LockablePropertyData> references = base.GetLockableProperties();
            foreach (LockablePropertyData propertyData in references)
            {
                propertyData.EndStepLocked = !Data.KeepUnlocked;
            }

            return references;
        }

        public override IProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }
    }
}