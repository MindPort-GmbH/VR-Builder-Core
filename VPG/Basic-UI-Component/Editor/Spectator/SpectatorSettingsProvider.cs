using UnityEditor;

namespace VPG.Editor.UI
{
    public class SpectatorSettingsProvider : BaseSettingsProvider
    {
        const string Path = "Project/VR Process Gizmo/Spectator";

        public SpectatorSettingsProvider() : base(Path, SettingsScope.Project)
        {
        }

        protected override void InternalDraw(string searchContext)
        {
        }

        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            SettingsProvider provider = new SpectatorSettingsProvider();
            return provider;
        }
    }
}
