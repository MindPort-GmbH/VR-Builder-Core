using System;
using UnityEngine;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Timestamp implementation of the <see cref="ValueProperty{T}"/> class.
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeValueProperty : ProcessSceneObjectProperty, IValueProperty<TimeSpan>
    {
        /// <summary>
        /// Default value set in the inspector.
        /// </summary>
        [SerializeField]
        protected TimeSpan defaultValue;

        /// <summary>
        /// Currently stored value.
        /// </summary>
        protected TimeSpan storedValue;

        private void Start()
        {
            ResetValue();
        }

        /// <summary>
        /// Returns the stored value.
        /// </summary>        
        public TimeSpan GetValue()
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
        public void SetValue(TimeSpan value)
        {
            storedValue = value;
        }
    }
}
