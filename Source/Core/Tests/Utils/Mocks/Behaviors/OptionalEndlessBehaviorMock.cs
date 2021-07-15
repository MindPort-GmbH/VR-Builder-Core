using VRBuilder.Core.Configuration.Modes;

namespace VRBuilder.Tests.Utils.Mocks
{
    public class OptionalEndlessBehaviorMock : EndlessBehaviorMock, IOptional
    {
        public OptionalEndlessBehaviorMock(bool isBlocking = true) : base(isBlocking)
        {
        }
    }
}
