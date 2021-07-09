using VRBuilder.Core.Properties;

namespace VRBuilder.Tests.Utils.Mocks
{
    public class LockablePropertyMock : LockableProperty, ILockablePropertyMock
    {
        protected override void InternalSetLocked(bool lockState)
        {
        }
    }
}
