using System.Runtime.Serialization;
using UnityEngine;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Base implementation for value properties.
    /// </summary>    
    public abstract class ValueProperty<T> : ProcessSceneObjectProperty, IValueProperty<T>
    {
        /// <summary>
        /// Default value set in the inspector.
        /// </summary>
        [SerializeField]
        protected T defaultValue;

        /// <summary>
        /// Currently stored value.
        /// </summary>
        protected T storedValue;

        private void Start()
        {
            ResetValue();
        }

        /// <summary>
        /// Returns the stored value.
        /// </summary>        
        public T GetValue()
        {
            return storedValue;
        }

        /// <summary>
        /// Resets the stored value to the default.
        /// </summary>
        public void ResetValue()
        {
            storedValue = defaultValue;
        }

        /// <summary>
        /// Sets the stored value to the specified value.
        /// </summary>        
        public void SetValue(T value)
        {
            storedValue= value;
        }
    }
}
