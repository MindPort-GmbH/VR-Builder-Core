using System.Collections.Generic;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.EntityOwners;

namespace VRBuilder.Core
{
    /// <summary>
    /// A data that contains a list of <see cref="IBehavior"/>s.
    /// </summary>
    public interface IBehaviorCollectionData : IEntityCollectionDataWithMode<IBehavior>
    {
        /// <summary>
        /// A list of <see cref="IBehavior"/>s.
        /// </summary>
        IList<IBehavior> Behaviors { get; set; }
    }
}
