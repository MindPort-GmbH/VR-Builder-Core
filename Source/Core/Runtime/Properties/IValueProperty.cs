namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Interface for a property that stores a single value.
    /// </summary>
    /// <typeparam name="T">Type of the value to be stored.</typeparam>
    public interface IValueProperty<T> : ISceneObjectProperty
    {
        /// <summary>
        /// Returns the value.
        /// </summary>
        T GetValue();

        /// <summary>
        /// Sets the value.
        /// </summary>
        void SetValue(T value);

        /// <summary>
        /// Resets the value to its default.
        /// </summary>
        void ResetValue();
    }
}
