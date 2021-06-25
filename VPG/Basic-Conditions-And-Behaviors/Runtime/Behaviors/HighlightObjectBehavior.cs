using System.Runtime.Serialization;
using VPG.Core.Attributes;
using VPG.Core.Configuration.Modes;
using VPG.Core.SceneObjects;
using VPG.Core.Properties;
using VPG.Core.Utils;
using UnityEngine;

namespace VPG.Core.Behaviors
{
    /// <summary>
    /// Behavior that highlights the target <see cref="ISceneObject"/> with the specified color until the behavior is being deactivated.
    /// </summary>
    [DataContract(IsReference = true)]
    [HelpLink("https://developers.innoactive.de/documentation/creator/latest/articles/innoactive-creator/default-behaviors.html#hightlight-object")]
    public class HighlightObjectBehavior : Behavior<HighlightObjectBehavior.EntityData>, IOptional
    {
        /// <summary>
        /// "Highlight object" behavior's data.
        /// </summary>
        [DisplayName("Highlight Object")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// <see cref="ModeParameter{T}"/> of the highlight color.
            /// Training modes can change the highlight color.
            /// </summary>
            public ModeParameter<Color> CustomHighlightColor { get; set; }

            /// <summary>
            /// Highlight color set in the Step Inspector.
            /// </summary>
            [DataMember]
            [DisplayName("Color")]
            public Color HighlightColor
            {
                get { return CustomHighlightColor.Value; }

                set { CustomHighlightColor = new ModeParameter<Color>("HighlightColor", value); }
            }

            /// <summary>
            /// Target scene object to be highlighted.
            /// </summary>
            [DataMember]
            [DisplayName("Object")]
            public ScenePropertyReference<IHighlightProperty> ObjectToHighlight { get; set; }

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
                Data.ObjectToHighlight.Value?.Highlight(Data.HighlightColor);
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
                Data.ObjectToHighlight.Value?.Unhighlight();
            }
        }

        private class EntityConfigurator : Configurator<EntityData>
        {
            /// <inheritdoc />
            public override void Configure(IMode mode, Stage stage)
            {
                Data.CustomHighlightColor.Configure(mode);
            }

            public EntityConfigurator(EntityData data) : base(data)
            {
            }
        }

        public HighlightObjectBehavior() : this("", Color.magenta)
        {
        }

        public HighlightObjectBehavior(string sceneObjectName, Color highlightColor, string name = "Highlight Object")
        {
            Data.ObjectToHighlight = new ScenePropertyReference<IHighlightProperty>(sceneObjectName);
            Data.HighlightColor = highlightColor;
            Data.Name = name;
        }

        public HighlightObjectBehavior(IHighlightProperty target) : this(target, Color.magenta)
        {
        }

        public HighlightObjectBehavior(IHighlightProperty target, Color highlightColor, string name = "Highlight Object") : this(TrainingReferenceUtils.GetNameFrom(target), highlightColor, name)
        {
        }

        /// <inheritdoc />
        public override IProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        /// <inheritdoc />
        public override IProcess GetDeactivatingProcess()
        {
            return new DeactivatingProcess(Data);
        }

        /// <inheritdoc />
        protected override IConfigurator GetConfigurator()
        {
            return new EntityConfigurator(Data);
        }
    }
}
