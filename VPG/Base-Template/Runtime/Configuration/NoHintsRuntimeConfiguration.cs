using System.Collections.Generic;
using VPG.Core.Behaviors;
using VPG.Core.Configuration;
using VPG.Core.Configuration.Modes;

namespace VPG.BaseTemplate.Configuration
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