using System.Collections.Generic;
using VPG.Core;
using VPG.Core.Internationalization;
using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Base class for the course controller menu.
    /// </summary>
    public abstract class BaseCourseControllerMenu : MonoBehaviour
    {
        private List<string> localizationFileNames;
        
        /// <summary>
        /// Localized file names.
        /// </summary>
        protected List<string> LocalizationFileNames
        {
            get
            {
                if (localizationFileNames == null)
                {
                    localizationFileNames = FetchAvailableLocalizationsForTraining();
                }

                return localizationFileNames;
            }
            set => localizationFileNames = value;
        }

        /// <summary>
        /// Returns all available localizations for the active training.
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> FetchAvailableLocalizationsForTraining()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add(LocalizationReader.KeyCourseName, CourseRunner.Current.Data.Name);
            return LocalizationUtils.FindAvailableLanguagesForConfig(GetLocalizationConfig(), parameters);
        }
        
        /// <summary>
        /// Returns the language which should be selected right now.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSelectedLanguage()
        {
            if (string.IsNullOrEmpty(LanguageSettings.Instance.ActiveLanguage) == false)
            {
                return LanguageSettings.Instance.ActiveLanguage;
            }

            if (LocalizationFileNames.Contains(LocalizationUtils.GetSystemLanguageAsTwoLetterIsoCode().ToLower()))
            {
                return LocalizationUtils.GetSystemLanguageAsTwoLetterIsoCode();
            }
            return LanguageSettings.Instance.DefaultLanguage;
        }
        
        /// <summary>
        /// Returns the used <see cref="LocalizationConfig"/>.
        /// </summary>
        protected virtual LocalizationConfig GetLocalizationConfig()
        {
            ICourseController controller = FindObjectOfType<CourseControllerSetup>().CurrentCourseController;
            if (controller is ILocalizationProvider localizationProvider)
            {
                return localizationProvider.LocalizationConfig;
            }
            
            return Resources.Load<LocalizationConfig>(LocalizationConfig.DefaultLocalizationConfig);
        }
    }
}