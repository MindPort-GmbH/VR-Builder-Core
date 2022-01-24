namespace VRBuilder.Core.Properties
{
    public interface IValueProperty<T> : ISceneObjectProperty
    {
        T GetValue();

        void SetValue(T value);
    }
}
