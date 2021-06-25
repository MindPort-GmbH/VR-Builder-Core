using System;
using System.Linq;
using System.Collections.Generic;
using VPG.Core.Audio;
using VPG.Core.Behaviors;
using VPG.Core.Properties;
using VPG.Core.Configuration;
using VPG.Core.Internationalization;
using VPG.Core.SceneObjects;

namespace VPG.Tests.Builder
{
    /// <summary>
    /// Basic step builder that creates step of type <typeparamref name="Step" />.
    /// </summary>
    public class BasicCourseStepBuilder : BasicStepBuilder
    {
        private const float defaultAudioDelay = 15f;

        #region private static methods
        private static ISceneObject GetFromRegistry(string name)
        {
            return RuntimeConfigurator.Configuration.SceneObjectRegistry[name];
        }
        #endregion

        protected bool IsAudioAddedOnlyManually { get; set; }

        protected bool IsAudioDescriptionAdded { get; set; }

        protected bool IsAudioSuccessAdded { get; set; }

        protected bool IsAudioHintAdded { get; set; }

        /// <summary>
        /// This builder will create step with given name.
        /// </summary>
        /// <param name="name">Name of a step.</param>
        public BasicCourseStepBuilder(string name) : base(name)
        {
            
        }
        
        #region public methods
        /// <inheritdoc cref="BuilderWithResourcePath{T}" />
        public new BasicCourseStepBuilder SetResourcePath(string path)
        {
            base.SetResourcePath(path);
            return this;
        }

        /// <summary>
        /// Play audio at the beginning of the step.
        /// </summary>
        /// <param name="path">Path to audio clip.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder AddAudioDescription(LocalizedString path)
        {
            AddSecondPassAction(() => AudioDescriptionAction(path.Clone()));
            return this;
        }

        /// <summary>
        /// Play audio at the end of the step.
        /// </summary>
        /// <param name="path">Path to audio clip.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder AddAudioSuccess(LocalizedString path)
        {
            AddSecondPassAction(() => AudioSuccessAction(path.Clone()));
            return this;
        }

        /// <summary>
        /// Play audio with a delay.
        /// </summary>
        /// <param name="path">Path to audioclip.</param>
        /// <param name="delayInSeconds">The delay between entering the step and playing the audio clip.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder AddAudioHint(LocalizedString path, float delayInSeconds = defaultAudioDelay)
        {
            AddSecondPassAction(() => AudioHintAction(path.Clone(), delayInSeconds));
            return this;
        }

        /// <summary>
        /// Enable game objects for the duration of the step.
        /// </summary>
        /// <param name="toEnable">Target objects.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder Enable(params ISceneObject[] toEnable)
        {
            AddSecondPassAction(() =>
            {
                foreach (ISceneObject trainingObject in toEnable)
                {
                    Result.Data.Behaviors.Data.Behaviors.Add(new EnableGameObjectBehavior(trainingObject));
                }
            });

            return this;
        }

        /// <summary>
        /// Enable game objects for the duration of the step.
        /// </summary>
        /// <param name="toEnable">Names of target objects.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder Enable(params string[] toEnable)
        {
            return Enable(toEnable.Select(GetFromRegistry).ToArray());
        }

        /// <summary>
        /// Disable game objects for the duration of the step.
        /// </summary>
        /// <param name="toDisable">List of objects to disable.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder Disable(params ISceneObject[] toDisable)
        {
            AddSecondPassAction(() =>
            {
                foreach (ISceneObject trainingObject in toDisable)
                {
                    Result.Data.Behaviors.Data.Behaviors.Add(new DisableGameObjectBehavior(trainingObject));
                }
            });
            return this;
        }

        /// <summary>
        /// Disable game objects for the duration of the step.
        /// </summary>
        /// <param name="toDisable">List of names of the objects to disable.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder Disable(params string[] toDisable)
        {
            return Disable(toDisable.Select(GetFromRegistry).ToArray());
        }

        /// <summary>
        /// Highlight objects.
        /// </summary>
        /// <param name="toHighlight">List of objects to highlight.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder Highlight(params string[] toHighlight)
        {
            return Highlight(toHighlight.Select(GetFromRegistry).ToArray());
        }

        /// <summary>
        /// Highlight objects.
        /// </summary>
        /// <param name="toHighlight">List of objects to highlight.</param>
        /// <returns>This.</returns>
        public BasicCourseStepBuilder Highlight(params ISceneObject[] toHighlight)
        {
            AddSecondPassAction(() =>
            {
                foreach (ISceneObject trainingObject in toHighlight)
                {
                    if (trainingObject.CheckHasProperty<IHighlightProperty>())
                    {
                        Result.Data.Behaviors.Data.Behaviors.Add(new HighlightObjectBehavior(trainingObject.GetProperty<IHighlightProperty>()));
                    }
                }
            });

            return this;
        }
        #endregion

        #region protected methods
        protected virtual void AudioDescriptionAction(LocalizedString path)
        {
            if (IsAudioDescriptionAdded)
            {
                throw new InvalidOperationException("AddAudioDescriptionAction can be called only once per step builder.");
            }

            IsAudioDescriptionAdded = true;

            Result.Data.Behaviors.Data.Behaviors.Add(new PlayAudioBehavior(new ResourceAudio(path.Clone()), BehaviorExecutionStages.Activation));
        }

        protected virtual void AudioSuccessAction(LocalizedString path)
        {
            if (IsAudioSuccessAdded)
            {
                throw new InvalidOperationException("AddAudioSuccessAction can be called only once per step builder.");
            }

            IsAudioSuccessAdded = true;

            Result.Data.Behaviors.Data.Behaviors.Add(new PlayAudioBehavior(new ResourceAudio(path.Clone()), BehaviorExecutionStages.Deactivation));
        }

        protected virtual void AudioHintAction(LocalizedString path, float delayInSeconds = defaultAudioDelay)
        {
            if (IsAudioHintAdded)
            {
                throw new InvalidOperationException("AddAudioHintAction can be called only once per step builder.");
            }

            IsAudioHintAdded = true;

            Result.Data.Behaviors.Data.Behaviors.Add(
                new BehaviorSequence(
                    false,
                    new List<IBehavior>
                    {
                        new DelayBehavior(delayInSeconds),
                        new PlayAudioBehavior(new ResourceAudio(path.Clone()), BehaviorExecutionStages.Activation)
                    },
                    false));
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            IsAudioAddedOnlyManually = false;

            IsAudioDescriptionAdded = false;
            IsAudioSuccessAdded = false;
            IsAudioHintAdded = false;
        }
        #endregion
    }
}
