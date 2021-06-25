using System.Collections.Generic;
using VPG.Core.Behaviors;
using VPG.TextToSpeech.Audio;
using VPG.Core.Internationalization;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.BaseTemplate.UI.Behaviors
{
    public class AudioHintMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "VPG/Audio Hint";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            DelayBehavior delayBehavior = new DelayBehavior(5f);
            delayBehavior.Data.Name = "Wait for";

            PlayAudioBehavior audioBehavior = new PlayAudioBehavior(new TextToSpeechAudio(new LocalizedString()), BehaviorExecutionStages.Activation);
            audioBehavior.Data.Name = "Play Audio";

            BehaviorSequence behaviorSequence = new BehaviorSequence(true, new List<IBehavior>
            {
                delayBehavior,
                audioBehavior
            });
            
            behaviorSequence.Data.Name = "Audio Hint";
            behaviorSequence.Data.IsBlocking = false;

            return behaviorSequence;
        }
    }
}
