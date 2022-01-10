// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.EntityOwners;

namespace VRBuilder.Core
{
    /// <summary>
    /// An implementation of <see cref="IProcess"/> class.
    /// It contains a complete information about the process workflow.
    /// </summary>
    [DataContract(IsReference = true)]
    public class Process : Entity<Process.EntityData>, IProcess
    {
        /// <summary>
        /// The data class for a process.
        /// </summary>
        public class EntityData : EntityCollectionData<IChapter>, IProcessData
        {
            /// <inheritdoc />
            [DataMember]
            public IList<IChapter> Chapters { get; set; }

            /// <inheritdoc />
            public IChapter FirstChapter
            {
                get { return Chapters[0]; }
            }

            /// <inheritdoc />
            public override IEnumerable<IChapter> GetChildren()
            {
                return Chapters.ToArray();
            }

            /// <inheritdoc />
            public IChapter Current { get; set; }

            /// <inheritdoc />
            [DataMember]
            [HideInProcessInspector]
            public string Name { get; set; }

            /// <inheritdoc />
            public IMode Mode { get; set; }
        }

        /// <summary>
        /// Step that is currently being executed.
        /// </summary>
        [DataMember]
        public IStep CurrentStep { get; protected set; }

        private class ActivatingProcess : EntityIteratingProcess<IChapter>
        {
            private IEnumerator<IChapter> enumerator;

            public ActivatingProcess(IEntitySequenceDataWithMode<IChapter> data) : base(data)
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
            protected override bool TryNext(out IChapter entity)
            {
                if (enumerator == null || (enumerator.MoveNext() == false))
                {
                    entity = default;
                    return false;
                }
                else
                {
                    entity = enumerator.Current;
                    return true;
                }
            }
        }

        /// <inheritdoc />
        IProcessData IDataOwner<IProcessData>.Data
        {
            get { return Data; }
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        /// <inheritdoc />
        public override IStageProcess GetDeactivatingProcess()
        {
            return new StopEntityIteratingProcess<IChapter>(Data);
        }

        protected Process() : this(null, new IChapter[0])
        {
        }

        public Process(string name, IChapter chapter) : this(name, new List<IChapter> { chapter })
        {
        }

        public Process(string name, IEnumerable<IChapter> chapters)
        {
            Data.Chapters = chapters.ToList();
            Data.Name = name;
        }
    }
}
