using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Exceptions
{
    public class NameNotUniqueException : TrainingException
    {
        public NameNotUniqueException(ISceneObject entity) : base(string.Format("Could not register Item with name '{0}', name already in use", entity.UniqueName))
        {
        }
    }
}
