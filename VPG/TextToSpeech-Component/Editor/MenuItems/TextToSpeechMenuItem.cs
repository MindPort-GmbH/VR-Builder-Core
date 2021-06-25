using VPG.Core.Behaviors;
using VPG.TextToSpeech.Audio;
using VPG.Core.Internationalization;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.TextToSpeech.UI.Behaviors
{
    /// <inheritdoc />
    public class TextToSpeechMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Audio/Play TextToSpeech Audio";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new PlayAudioBehavior(new TextToSpeechAudio(new LocalizedString()), BehaviorExecutionStages.Activation, true);
        }
    }
}
