using System.Collections.ObjectModel;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.Serialization;
using VRBuilder.Editor.UI.StepInspector.Menu;

namespace VRBuilder.Editor.Configuration
{
    internal class EditorConfigWrapper : DefaultEditorConfiguration
    {
        private IEditorConfiguration config;

        public EditorConfigWrapper(IEditorConfiguration config)
        {
            this.config = config;
        }

        public override ICourseSerializer Serializer => config.Serializer;

        public override AllowedMenuItemsSettings AllowedMenuItemsSettings
        {
            get => config.AllowedMenuItemsSettings;
            set => config.AllowedMenuItemsSettings = value;
        }

        public override string CourseStreamingAssetsSubdirectory => config.CourseStreamingAssetsSubdirectory;

        public override string AllowedMenuItemsSettingsAssetPath => config.AllowedMenuItemsSettingsAssetPath;

        public override ReadOnlyCollection<MenuOption<IBehavior>> BehaviorsMenuContent => config.BehaviorsMenuContent;

        public override ReadOnlyCollection<MenuOption<ICondition>> ConditionsMenuContent => config.ConditionsMenuContent;

    }
}
