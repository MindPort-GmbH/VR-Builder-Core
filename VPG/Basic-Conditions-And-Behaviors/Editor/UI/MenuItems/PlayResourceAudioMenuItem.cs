using VPG.Core.Audio;
using VPG.Core.Behaviors;
using VPG.Core.Internationalization;
using VPG.Editor.UI.StepInspector.Menu;

namespace VPG.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class PlayResourceAudioMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Audio/Play Audio File";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new PlayAudioBehavior(new ResourceAudio(new LocalizedString()), BehaviorExecutionStages.Activation, true);
        }
    }
}
