using System.Collections.ObjectModel;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.Serialization;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.Configuration
{
    /// <summary>
    /// Interface for editor configuration definition. Implement it to create your own.
    /// </summary>
    public interface IEditorConfiguration
    {
        /// <summary>
        /// Absolute path where training courses are stored.
        /// </summary>
        string CourseStreamingAssetsSubdirectory { get; }

        /// <summary>
        /// Assets path where to save the serialized <see cref="AllowedMenuItemsSettings"/> file.
        /// It has to start with "Assets/".
        /// </summary>
        string AllowedMenuItemsSettingsAssetPath { get; }

        /// <summary>
        /// Serializer used to serialize training courses and steps.
        /// </summary>
        ICourseSerializer Serializer { get; }

        /// <summary>
        /// The current instance of the <see cref="AllowedMenuItemsSettings"/> object.
        /// It manages the display status of all available behaviors and conditions.
        /// </summary>
        AllowedMenuItemsSettings AllowedMenuItemsSettings { get; set; }

        /// <summary>
        /// List of available options in "Add new behavior" dropdown.
        /// </summary>
        ReadOnlyCollection<MenuOption<IBehavior>> BehaviorsMenuContent { get; }

        /// <summary>
        /// List of available options in "Add new condition" dropdown.
        /// </summary>
        ReadOnlyCollection<MenuOption<ICondition>> ConditionsMenuContent { get; }
    }
}
