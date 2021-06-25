using System;
using System.IO;

namespace VPG.Editor.PackageManager.BasicTemplate
{
    /// <summary>
    /// Adds Unity's Text Mesh Pro package as a dependency and prompts the 'TMP Importer' window if the 'TMP Essentials' are missing in the project.
    /// </summary>
    public class TextMeshProPackageEnabler : Dependency, IDisposable
    {
        private const string TMPEssentialResourcesPath = "Assets/TextMesh Pro";
        private const string TMPSettingsAssemblyQualifiedName = "TMPro.TMP_Settings, Unity.TextMeshPro, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        private const string TMPSettingsMethodName = "GetSettings";

        /// <inheritdoc/>
        public override string Package { get; } = "com.unity.textmeshpro";
        
        /// <inheritdoc/>
        public override int Priority { get; } = 50;

        public TextMeshProPackageEnabler()
        {
            if (Directory.Exists(TMPEssentialResourcesPath) == false)
            {
                OnPackageEnabled += ImportTMPSettings;
            }
        }
        
        public void Dispose()
        {
            OnPackageEnabled -= ImportTMPSettings;
        }

        private void ImportTMPSettings(object sender, EventArgs e)
        {
            Type tmpSettings = Type.GetType(TMPSettingsAssemblyQualifiedName);
            tmpSettings?.GetMethod(TMPSettingsMethodName)?.Invoke(null, null);
            
            OnPackageEnabled -= ImportTMPSettings;
        }
    }
}
