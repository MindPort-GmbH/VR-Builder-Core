using System;
using UnityEngine;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Property for <see cref="TimeSpan"/> values.
    /// </summary>    
    public abstract class TimeSpanValueProperty : ProcessSceneObjectProperty, IValueProperty<TimeSpan>
    {
        /// <summary>
        /// Default value set in the inspector.
        /// </summary>
        [SerializeField]
        protected int hours;

        [SerializeField]
        protected int minutes;

        [SerializeField]
        protected int seconds;

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
            storedValue = new TimeSpan(hours, minutes, seconds);
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
