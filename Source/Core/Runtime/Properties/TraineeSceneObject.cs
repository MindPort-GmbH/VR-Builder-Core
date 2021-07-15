using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Used to identify a trainee within the scene.
    /// </summary>
    public class TraineeSceneObject : TrainingSceneObject
    {
        protected new void Awake()
        {
            base.Awake();
            ChangeUniqueName("Trainee");
        }
    }
}
