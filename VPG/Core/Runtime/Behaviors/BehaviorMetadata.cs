using System.Runtime.Serialization;

namespace VRBuilder.Core.Behaviors
{
    [DataContract(IsReference = true)]
    public class BehaviorMetadata : IMetadata
    {
        [DataMember]
        public bool IsFoldedOut { get; set; }
    }
}
