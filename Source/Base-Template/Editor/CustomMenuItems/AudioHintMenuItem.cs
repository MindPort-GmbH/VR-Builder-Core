using System.Collections.Generic;
using VRBuilder.Core.Behaviors;
using VRBuilder.TextToSpeech.Audio;
using VRBuilder.Core.Internationalization;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.BaseTemplate.UI.Behaviors
{
    public class AudioHintMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "VR Builder/Audio Hint";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            DelayBehavior delayBehavior = new DelayBehavior(5f);
            delayBehavior.Data.Name = "Wait for";

            PlayAudioBehavior audioBehavior = new PlayAudioBehavior(new TextToSpeechAudio(""), BehaviorExecutionStages.Activation);
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
