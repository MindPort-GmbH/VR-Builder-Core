using VRBuilder.Core.Internationalization;

namespace VRBuilder.UX
{
    /// <summary>
    /// Interface which provides a <see cref="LocalizationConfig"/>.
    /// </summary>
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Returns the localization config which should be used.
        /// </summary>
        LocalizationConfig LocalizationConfig { get; }
    }
}