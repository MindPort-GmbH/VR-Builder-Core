using System;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Core.Properties;
using VRBuilder.Core.Utils;
using VRBuilder.Core.Validation;
using VRBuilder.Unity;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// Condition that is completed when distance between `Target` and `TransformInRangeDetector` is closer than `range` units.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-conditions.html#object-nearby")]
    public class ObjectInRangeCondition : Condition<ObjectInRangeCondition.EntityData>
    {
        /// <summary>
        /// The data of "object in range" condition.
        /// </summary>
        [DisplayName("Object Nearby")]
        public class EntityData : IObjectInTargetData
        {
            /// <summary>
            /// The tracked object.
            /// </summary>
            [DataMember]
            [DisplayName("Object")]
            public SceneObjectReference Target { get; set; }

            private ScenePropertyReference<TransformInRangeDetectorProperty> referenceProperty;

            /// <summary>
            /// The object to measure distance from.
            /// </summary>
            [DataMember]
            [DisplayName("Reference object")]
            public ScenePropertyReference<TransformInRangeDetectorProperty> ReferenceProperty
            {
                get
                {
#pragma warning disable 618
                    if ((referenceProperty == null || referenceProperty.IsEmpty()) && DistanceDetector != null && DistanceDetector.IsEmpty() == false)
                    {
                        DistanceDetector.Value.GameObject.GetOrAddComponent<TransformInRangeDetectorProperty>();
                        referenceProperty = new ScenePropertyReference<TransformInRangeDetectorProperty>(DistanceDetector.UniqueName);
                        DistanceDetector = null;
                    }
#pragma warning restore 618

                    return referenceProperty;
                }

                set => referenceProperty = value;
            }

            /// <summary>
            /// The object to measure distance from.
            /// </summary>
            [HideInProcessInspector]
            [Obsolete("Use 'ReferenceProperty' instead.")]
            public SceneObjectReference DistanceDetector;

            /// <summary>
            /// The required distance between two objects to trigger the condition.
            /// </summary>
            [DataMember]
            public float Range { get; set; }

            /// <inheritdoc />
            [DataMember]
            [HideInProcessInspector]
            public string Name { get; set; }

            /// <inheritdoc />
#if CREATOR_PRO
            [OptionalValue]
#endif
            [DataMember]
            [DisplayName("Required seconds inside")]
            public float RequiredTimeInside { get; set; }

            /// <inheritdoc />
            public bool IsCompleted { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }
        }

        public ObjectInRangeCondition() : this("", "", 0f)
        {
        }

        public ObjectInRangeCondition(ISceneObject target, TransformInRangeDetectorProperty detector, float range, float requiredTimeInTarget = 0, string name = null)
            : this(ProcessReferenceUtils.GetNameFrom(target), ProcessReferenceUtils.GetNameFrom(detector), range, requiredTimeInTarget, name)
        {
        }

        public ObjectInRangeCondition(string target, string detector, float range, float requiredTimeInTarget = 0, string name = "Object Nearby")
        {
            Data.Target = new SceneObjectReference(target);
            Data.ReferenceProperty = new ScenePropertyReference<TransformInRangeDetectorProperty>(detector);
            Data.Range = range;
            Data.RequiredTimeInside = requiredTimeInTarget;
            Data.Name = name;
        }

        private class ActiveProcess : ObjectInTargetActiveProcess<EntityData>
        {
            public ActiveProcess(EntityData data) : base(data)
            {
            }

            public override void Start()
            {
                Data.ReferenceProperty.Value.SetTrackedTransform(Data.Target.Value.GameObject.transform);
                Data.ReferenceProperty.Value.DetectionRange = Data.Range;
                
                base.Start();
            }

            /// <inheritdoc />
            protected override bool IsInside()
            {
                return Data.ReferenceProperty.Value.IsTargetInsideRange();
            }
        }

        private class EntityAutocompleter : Autocompleter<EntityData>
        {
            public EntityAutocompleter(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Complete()
            {
                Data.Target.Value.GameObject.transform.position = Data.ReferenceProperty.Value.gameObject.transform.position;
            }
        }

        /// <inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        /// <inheritdoc />
        protected override IAutocompleter GetAutocompleter()
        {
            return new EntityAutocompleter(Data);
        }
    }
}
