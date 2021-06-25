using System;
using VPG.Core.SceneObjects;
using VPG.Core.Properties;

namespace VPG.BasicInteraction.Properties
{
    public interface IUsableProperty : ISceneObjectProperty, ILockable
    {
        event EventHandler<EventArgs> UsageStarted;
        event EventHandler<EventArgs> UsageStopped;
        
        /// <summary>
        /// Is object currently used.
        /// </summary>
        bool IsBeingUsed { get; }
        
        /// <summary>
        /// Instantaneously simulate that the object was used.
        /// </summary>
        void FastForwardUse();
        
    }
}
