using System;
using VPG.Core.SceneObjects;
using VPG.Core.Properties;

namespace VPG.BasicInteraction.Properties
{
    public interface IGrabbableProperty : ISceneObjectProperty, ILockable
    {
        event EventHandler<EventArgs> Grabbed;
        event EventHandler<EventArgs> Ungrabbed;
        
        /// <summary>
        /// Is object currently grabbed.
        /// </summary>
        bool IsGrabbed { get; }
        
        /// <summary>
        /// Instantaneously simulate that the object was grabbed.
        /// </summary>
        void FastForwardGrab();

        /// <summary>
        /// Instantaneously simulate that the object was ungrabbed.
        /// </summary>
        void FastForwardUngrab();
    }
}
