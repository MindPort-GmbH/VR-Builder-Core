using System;

namespace VRBuilder.Core.SceneObjects
{
    public class LockStateChangedEventArgs : EventArgs
    {
        public readonly bool IsLocked;

        public LockStateChangedEventArgs(bool isLocked)
        {
            IsLocked = isLocked;
        }
    }
}
