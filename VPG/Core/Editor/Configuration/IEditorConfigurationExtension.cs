using System;
using System.Collections.Generic;

namespace VPG.Editor.Configuration
{
    /// <summary>
    /// Interface for editor configuration extension definition.
    /// </summary>
    public interface IEditorConfigurationExtension
    {      
        /// <summary>
        /// Menu items required by this configuration.
        /// </summary>
        IEnumerable<Type> RequiredMenuItems { get; }

        /// <summary>
        /// Menu items disabled by this configuration.
        /// </summary>
        IEnumerable<Type> DisabledMenuItems { get; }
    }
}
