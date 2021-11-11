using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.EntityOwners;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// A collection of behaviors that are activated and deactivated after each other.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-behaviors.html#behavior-sequence")]
    public class BehaviorSequence : Behavior<BehaviorSequence.EntityData>
    {
        /// <summary>
        /// Behavior sequence's data.
        /// </summary>
        [DisplayName("Behavior Sequence")]
        [DataContract(IsReference = true)]
        public class EntityData : EntityCollectionData<IBehavior>, IEntitySequenceDataWithMode<IBehavior>, IBackgroundBehaviorData
        {
            /// <summary>
            /// Are child behaviors activated only once or the collection is cycled.
            /// </summary>
            [DisplayName("Repeat")]
            [DataMember]
            public bool PlaysOnRepeat { get; set; }

            /// <summary>
            /// List of child behaviors.
            /// </summary>
            [DataMember]
            [DisplayName("Child behaviors")]
            [Foldable, ReorderableListOf(typeof(FoldableAttribute), typeof(DeletableAttribute), typeof(HelpAttribute)), ExtendableList]
            public List<IBehavior> Behaviors { get; set; }

            /// <inheritdoc />
            public override IEnumerable<IBehavior> GetChildren()
            {
                return Behaviors.ToList();
            }

            /// <inheritdoc />
            public IBehavior Current { get; set; }


            /// <inheritdoc />
            public string Name { get; set; }

            /// <inheritdoc />
            public IMode Mode { get; set; }

            /// <inheritdoc />
            public bool IsBlocking { get; set; }
        }

        private class IteratingProcess : EntityIteratingProcess<IBehavior>
        {
            private IEnumerator<IBehavior> enumerator;


            public IteratingProcess(IEntitySequenceDataWithMode<IBehavior> data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                base.Start();
                enumerator = Data.GetChildren().GetEnumerator();
            }

            /// <inheritdoc />
            protected override bool ShouldActivateCurrent()
            {
                return true;
            }

            /// <inheritdoc />
            protected override bool ShouldDeactivateCurrent()
            {
                return true;
            }

            /// <inheritdoc />
            protected override bool TryNext(out IBehavior entity)
            {
                if (enumerator == null || (enumerator.MoveNext() == false))
                {
                    entity = default(IBehavior);
                    return false;
                }
                else
                {
                    entity = enumerator.Current;
                    return true;
                }
            }
        }

        private class ActiveProcess : StageProcess<EntityData>
        {
            private readonly IStageProcess childProcess;

            public ActiveProcess(EntityData data) : base(data)
            {
                childProcess = new IteratingProcess(Data);
            }

            /// <inheritdoc />
            public override void Start()
            {
                childProcess.Start();
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                if (Data.PlaysOnRepeat == false)
                {
                    yield break;
                }

                int endlessLoopCheck = 0;

                while (endlessLoopCheck < 100000)
                {
                    IEnumerator update = childProcess.Update();

                    while (update.MoveNext())
                    {
                        yield return null;
                    }

                    childProcess.End();

                    childProcess.Start();

                    endlessLoopCheck++;
                }
            }

            /// <inheritdoc />
            public override void End()
            {
            }

            /// <inheritdoc />
            public override void FastForward()
            {
                childProcess.FastForward();
                childProcess.End();
            }
        }

        public BehaviorSequence() : this(default(bool), new List<IBehavior>())
        {
        }

        public BehaviorSequence(bool playsOnRepeat, IList<IBehavior> behaviors, string name = "Sequence")
        {
            Data.PlaysOnRepeat = playsOnRepeat;
            Data.Behaviors = new List<IBehavior>(behaviors);
            Data.Name = name;
            Data.IsBlocking = true;
        }

        public BehaviorSequence(bool playsOnRepeat, IList<IBehavior> behaviors, bool isBlocking, string name = "Sequence") : this(playsOnRepeat, behaviors, name)
        {
            Data.IsBlocking = isBlocking;
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new IteratingProcess(Data);
        }

        /// <inheritdoc />
        public override IStageProcess GetActiveProcess()
        {
            return new ActiveProcess(Data);
        }

        /// <inheritdoc />
        public override IStageProcess GetDeactivatingProcess()
        {
            return new StopEntityIteratingProcess<IBehavior>(Data);
        }

        /// <inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new SequenceConfigurator<IBehavior>(Data);
        }
    }
}
