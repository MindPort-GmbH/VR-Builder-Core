namespace VRBuilder.Core.Configuration
{
    public interface IInteractionComponentConfiguration
    {
        string DisplayName { get; }

        bool IsXRInteractionComponent { get; }
    }
}
