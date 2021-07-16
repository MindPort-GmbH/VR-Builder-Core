using VRBuilder.Core.Audio;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Internationalization;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.UI.Behaviors
{
    /// <inheritdoc />
    public class PlayResourceAudioMenuItem : MenuItem<IBehavior>
    {
        /// <inheritdoc />
        public override string DisplayedName { get; } = "Audio/Play Audio File";

        /// <inheritdoc />
        public override IBehavior GetNewItem()
        {
            return new PlayAudioBehavior(new ResourceAudio(""), BehaviorExecutionStages.Activation, true);
        }
    }
}
