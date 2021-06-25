#if UNITY_XR_MANAGEMENT && OPEN_XR
namespace VPG.Editor.XRUtils
{
    /// <summary>
    /// Enables the Windows MR Plugin.
    /// </summary>
    internal sealed class OpenXRPackageEnabler : XRProvider
    {
        /// <inheritdoc/>
        public override string Package { get; } = "com.unity.xr.openxr";

        /// <inheritdoc/>
        public override int Priority { get; } = 2;

        protected override string XRLoaderName { get; } = "OpenXRLoader";
    }
}
#endif
