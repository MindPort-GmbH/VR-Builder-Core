using VRBuilder.Core.Configuration.Modes;

namespace VRBuilder.Core.EntityOwners
{
    /// <summary>
    /// An <seealso cref="IEntitySequenceData{TEntity}"/> with <seealso cref="IModeData"/>.
    /// </summary>
    public interface IEntitySequenceDataWithMode<TEntity> : IEntitySequenceData<TEntity>, IModeData where TEntity : IEntity
    {
    }
}
