using VPG.Core.SceneObjects;
using VPG.Core.Properties;

namespace VPG.BasicInteraction.Properties
{
    public interface ITouchableProperty : ISceneObjectProperty, ILockable
    {
        /// <summary>
        /// Is object currently touched.
        /// </summary>
        bool IsBeingTouched { get; }
        
        /// <summary>
        /// Instantaneously simulate that the object was touched.
        /// </summary>
        void FastForwardTouch();
    }
}