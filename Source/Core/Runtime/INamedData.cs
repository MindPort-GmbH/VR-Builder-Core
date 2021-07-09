using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;

namespace VRBuilder.Core
{
    /// <summary>
    /// Data structure with an <see cref="IStep"/>'s name.
    /// </summary>
    public interface INamedData : IData
    {
        /// <summary>
        /// <see cref="IStep"/>'s name.
        /// </summary>
        [DataMember]
        [HideInTrainingInspector]
        string Name { get; set; }
    }
}
