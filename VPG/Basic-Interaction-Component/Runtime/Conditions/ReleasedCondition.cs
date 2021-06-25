using System.Runtime.Serialization;
using VPG.BasicInteraction.Properties;
using VPG.Core;
using VPG.Core.Attributes;
using VPG.Core.Conditions;
using VPG.Core.SceneObjects;
using VPG.Core.Utils;
using VPG.Core.Validation;

namespace VPG.BasicInteraction.Conditions
{
    /// <summary>
    /// Condition which is completed when `GrabbableProperty` becomes ungrabbed.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html#release-object")]
    public class ReleasedCondition : Condition<ReleasedCondition.EntityData>
    {
        [DisplayName("Release Object")]
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

            public Metadata Metadata { get; set; }
        }

        private class ActiveProcess : BaseActiveProcessOverCompletable<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            protected override bool CheckIfCompleted()
            {
                return Data.GrabbableProperty.Value.IsGrabbed == false;
            }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            public override void Complete()
            {
                Data.GrabbableProperty.Value.FastForwardUngrab();
            }
        }

        public ReleasedCondition() : this("")
        {
        }

        public ReleasedCondition(IGrabbableProperty target, string name = null) : this(TrainingReferenceUtils.GetNameFrom(target), name)
        {
        }

        public ReleasedCondition(string target, string name = "Release Object")
        {
            Data.GrabbableProperty = new ScenePropertyReference<IGrabbableProperty>(target);
            Data.Name = name;
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