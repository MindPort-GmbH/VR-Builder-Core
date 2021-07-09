using System.Collections.Generic;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Configuration.Modes;

namespace VRBuilder.BaseTemplate.Configuration
{
    public class NoHintsRuntimeConfiguration : DefaultRuntimeConfiguration
    {
        protected NoHintsRuntimeConfiguration()
        {
            IMode noHints = new Mode("No Audio Hints", new WhitelistTypeRule<IOptional>().Add<PlayAudioBehavior>());
            Modes = new BaseModeHandler(new List<IMode> { DefaultMode, noHints });
        }
    }
}