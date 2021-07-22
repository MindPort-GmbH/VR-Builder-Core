using VRBuilder.Core.Configuration.Modes;

namespace VRBuilder.Tests.Utils.Mocks
{
    /// <summary>
    /// Same as <see cref="EndlessConditionMock"/>, but it can be skipped.
    /// </summary>
    public class OptionalEndlessConditionMock : EndlessConditionMock, IOptional
    {
    }
}
