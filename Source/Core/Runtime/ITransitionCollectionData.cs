using System.Collections.Generic;
using VRBuilder.Core.EntityOwners;

namespace VRBuilder.Core
{
    /// <summary>
    /// The interface of a data with a list of <see cref="ITransition"/>s.
    /// </summary>
    public interface ITransitionCollectionData : IEntityCollectionDataWithMode<ITransition>
    {
        /// <summary>
        /// A list of <see cref="ITransition"/>s.
        /// </summary>
        IList<ITransition> Transitions { get; set; }
    }
}
