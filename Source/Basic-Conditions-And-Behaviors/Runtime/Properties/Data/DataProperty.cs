using System;
using UnityEngine;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Base implementation for process data properties.
    /// </summary>    
    [DisallowMultipleComponent]
    public abstract class DataProperty<T> : ProcessSceneObjectProperty, IDataProperty<T> where T : IEquatable<T>
    {
        /// <inheritdoc/>
        [SerializeField]
        protected T defaultValue;

        /// <inheritdoc/>
        protected T storedValue;

        /// <inheritdoc/>
        public event EventHandler<EventArgs> ValueChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs> ValueReset;

        private void Awake()
        {
            ResetValue();
        }

        /// <inheritdoc/>
        public T GetValue()
        {
            return storedValue;
        }

        /// <inheritdoc/>
        public void ResetValue()
        {
            SetValue(defaultValue);
            ValueReset?.Invoke(this, EventArgs.Empty); 
        }

        /// <inheritdoc/>
        public void SetValue(T value)
        {
            if((storedValue == null && value == null) || value.Equals(storedValue))
            {
                return;
            }

            storedValue = value;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
