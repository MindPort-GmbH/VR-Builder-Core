using TMPro;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VPG.Core;
using VPG.Unity;
using VPG.Core.IO;
using VPG.TextToSpeech;
using VPG.Core.Configuration;
using VPG.Core.Configuration.Modes;
using VPG.Core.Internationalization;

namespace VPG.UX
{
    /// <summary>
    /// Standalone controller class for an example of a custom training overlay with audio and localization.
    /// </summary>
    public class StandaloneCourseMenu : BaseCourseControllerMenu
    {
        #region UI elements
        [Tooltip("Chapter picker dropdown.")]
        [SerializeField]
        private TMP_Dropdown chapterPicker = null;

        [Tooltip("The image next to a step name which is visible when a training is running.")]
        [SerializeField]
        private Image trainingStateIndicator = null;

        [Tooltip("Name of the step that is currently executed.")]
        [SerializeField]
        private TextMeshProUGUI stepName = null;

        [Tooltip("Button that shows additional information about the step.")]
        [SerializeField]
        private Toggle stepInfoToggle = null;

        [Tooltip("Background for additional step information.")]
        [SerializeField]
        private Image stepInfoBackground = null;

        [Tooltip("Short description of the text which is visible when Info Toggle is toggled on.")]
        [SerializeField]
        private TextMeshProUGUI stepInfoText = null;

        [Tooltip("Button that starts execution of the training.")]
        [SerializeField]
        private Button startTrainingButton = null;

        [Tooltip("Step picker dropdown.")]
        [SerializeField]
        private TMP_Dropdown skipStepPicker = null;

        [Tooltip("Button that resets the scene to its initial state.")]
        [SerializeField]
        private Button resetSceneButton = null;

        [Tooltip("Toggle that turns training audio on or off.")]
        [SerializeField]
        private Toggle soundToggle = null;

        [Tooltip("Image that shows the sound icon.")]
        [SerializeField]
        private Image soundImage = null;
        
        [Tooltip("Icon that indicates that sound is enabled.")]
        [SerializeField]
        private Sprite soundOnImage = null;

        [Tooltip("Icon that indicates that sound is disabled.")]
        [SerializeField]
        private Sprite soundOffImage = null;

        [Tooltip("Language picker dropdown.")]
        [SerializeField]
        private TMP_Dropdown languagePicker = null;

        [Tooltip("Mode picker dropdown.")]
        [SerializeField]
        private TMP_Dropdown modePicker = null;
        #endregion

        [Tooltip("The two-letter ISO language code (e.g. \"EN\") of the fallback language which is used by the text to speech engine if no valid localization file is found.")]
        [SerializeField]
        private string fallbackLanguage = "EN";

        private string selectedLanguage;
        private FieldInfo skipStepPickerEditorValueField;

        private IStep displayedStep;
        private ICourse trainingCourse;
        private IChapter lastDisplayedChapter;

        private void Awake()
        {
            selectedLanguage = GetSelectedLanguage();
            LanguageSettings.Instance.ActiveLanguage = selectedLanguage;
            
            // Load the localization for the current selected course.
            LoadLocalizationForTraining(RuntimeConfigurator.Instance.GetSelectedCourse());
            SetupTraining();
            
            // Setup UI controls.
            SetupChapterPicker();
            SetupStepInfoToggle();
            SetupStartTrainingButton();
            SetupSkipStepPicker();
            SetupResetSceneButton();
            SetupSoundToggle();
            SetupLanguagePicker();
            SetupModePicker();
            
            // Update the UI.
            SetupTrainingDependantUI();
        }

        private void Update()
        {
            IChapter currentChapter = CourseRunner.Current == null ? null : CourseRunner.Current.Data.Current;
            IStep currentStep = currentChapter?.Data.Current;
            
            if (currentChapter != lastDisplayedChapter)
            {
                lastDisplayedChapter = currentChapter;
                UpdateDisplayedChapter(currentChapter);
            }

            if (currentStep != displayedStep)
            {
                displayedStep = currentStep;
                UpdateDisplayedStep(currentStep);
            }
        }

        private void UpdateDisplayedStep(IStep step)
        {
            if (step == null)
            {
                // If there is no next step, clear the info text.
                stepInfoText.text = string.Empty;
                stepName.text = string.Empty;
            }
            else
            {
                // Else, assign the description of the new step.
                stepInfoText.text = step.Data.Description;
                stepName.text = step.Data.Name;
            }

            SetupSkipStepPickerOptions();
        }

        private void UpdateDisplayedChapter(IChapter chapter)
        {
            // Get a collection of available chapters.
            IList<IChapter> chapters = CourseRunner.Current == null ? new List<IChapter>() : CourseRunner.Current.Data.Chapters.ToList();

            // Skip all finished chapters.
            int startingIndex = chapter == null ? 0 : chapters.IndexOf(chapter);

            // Show the rest.
            PopulateChapterPickerOptions(startingIndex);
        }

        private void SetupTraining()
        {
            // Load training course from a file.
            string coursePath = RuntimeConfigurator.Instance.GetSelectedCourse();
            
            // Load the localization file of the current selected language.
            LoadLocalizationForTraining(coursePath);
            
            // Try to load the in the [TRAINING_CONFIGURATION] selected training course.
            try
            {
                trainingCourse = RuntimeConfigurator.Configuration.LoadCourse(coursePath);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{exception.GetType().Name}, {exception.Message}\n{exception.StackTrace}", RuntimeConfigurator.Instance.gameObject);
                return;
            }
            
            // Initializes the training course. That will synthesize an audio for the training instructions, too.
            CourseRunner.Initialize(trainingCourse);
        }

        protected virtual void LoadLocalizationForTraining(string coursePath)
        {
            string course = Path.GetFileNameWithoutExtension(coursePath);

            LanguageSettings.Instance.ActiveLanguage = selectedLanguage;
            // Find the correct file name of the current selected language.
            string language = LocalizationFileNames.Find(f => string.Equals(f, selectedLanguage, StringComparison.CurrentCultureIgnoreCase));
            
            Localization.LoadLocalization(GetLocalizationConfig(), language, course);
        }
        
        protected override LocalizationConfig GetLocalizationConfig()
        {
            ICourseController controller = FindObjectOfType<CourseControllerSetup>().CurrentCourseController;
            if (controller is ILocalizationProvider localizationProvider)
            {
                return localizationProvider.LocalizationConfig;
            }
            
            return Resources.Load<LocalizationConfig>(LocalizationConfig.StandaloneDefaultLocalizationConfig);
        }

        private void FastForwardChapters(int numberOfChapters)
        {
            // Skip if no chapters have to be fast-forwarded.
            if (numberOfChapters == 0)
            {
                return;
            }

            CourseRunner.SkipChapters(numberOfChapters);
        }
        
        #region Setup UI
        private void SetupChapterPicker()
        {
            // When selected chapter has changed,
            chapterPicker.onValueChanged.AddListener(index =>
            {
                if (CourseRunner.IsRunning == false)
                {
                    return;
                }

                // If the training hasn't started it, ignore it. We will use this value when the training starts.
                if (CourseRunner.Current.LifeCycle.Stage == Stage.Inactive)
                {
                    return;
                }

                // Otherwise, fast forward the chapters until the selected is active.
                FastForwardChapters(index);
            });
        }

        private void SetupStepInfoToggle()
        {
            // When info toggle is pressed,
            stepInfoToggle.onValueChanged.AddListener(newValue =>
            {
                if (string.IsNullOrEmpty(stepInfoText.text))
                {
                    // Show or hide description of the step.
                    stepInfoBackground.enabled = false;
                    stepInfoText.enabled = false;
                    return;
                }
                
                // Show or hide description of the step.
                stepInfoBackground.enabled = newValue;
                stepInfoText.enabled = newValue;
            });
        }

        private void SetupStartTrainingButton()
        {
            // When user clicks on Start Training button,
            startTrainingButton.onClick.AddListener(() =>
            {
                if (CourseRunner.Current == null)
                {
                    Debug.LogError("No training course is selected.", RuntimeConfigurator.Instance.gameObject);
                    return;
                }
                
                // Subscribe to the "stage changed" event of the current training in order to change the skip step button to the start button after finishing the training.
                CourseRunner.Current.LifeCycle.StageChanged += (sender, args) =>
                {
                    if (args.Stage == Stage.Inactive)
                    {
                        skipStepPicker.gameObject.SetActive(false);
                        startTrainingButton.gameObject.SetActive(true);
                    }
                };

                //Skip all chapters before selected.
                FastForwardChapters(chapterPicker.value);

                // Start the training
                CourseRunner.Run();
                
                // Show the skip step button instead of the start button.
                skipStepPicker.gameObject.SetActive(true);
                startTrainingButton.gameObject.SetActive(false);
                
                // Disable button as you have to reset scene before starting the training again.
                startTrainingButton.interactable = false;
                // Disable the language picker as it is not allowed to change the language during the training's execution.
                languagePicker.interactable = false;
            });
        }

        private void SetupSkipStepPicker()
        {
            // Dropdown.onValueChanged won't be call if Dropdown.value is equal to the selected value. 
            // Dropdown.value can't be less than 0 or grater than Dropdown.options.Count -1.
            // This causes the dropdown to never call onValueChanged in cases when there is only 1 transition.
            // By setting the value to be out of range from the "editor" instead than from Dropdown.value we ensure that Dropdown.onValueChanged is always called.
            skipStepPickerEditorValueField = skipStepPicker.GetType().GetField("m_Value", BindingFlags.NonPublic | BindingFlags.Instance);
            
            // When a target step was chosen,
            skipStepPicker.onValueChanged.AddListener(index =>
            {
                // If there's an active step and it's not the last step,
                if (displayedStep != null && index < displayedStep.Data.Transitions.Data.Transitions.Count && displayedStep.LifeCycle.Stage != Stage.Inactive)
                {
                    CourseRunner.SkipStep(displayedStep.Data.Transitions.Data.Transitions[index]);
                }
            });
        }

        private void SetupResetSceneButton()
        {
            // When user clicks on Reset Scene button,
            resetSceneButton.onClick.AddListener(() =>
            {
                // Stop all coroutines. The Training SDK is in closed beta stage, and we can't properly interrupt a training yet.
                // For example, consider the Move Object behavior: it changes position of an object over time.
                // Even if you would reload the scene, it would still be moving that object, which will lead to unwanted result.
                CoroutineDispatcher.Instance.StopAllCoroutines();

                // Reload current scene.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            });
        }

        private void SetupSoundToggle()
        {
            // When sound toggle is clicked,
            soundToggle.onValueChanged.AddListener(isSoundOn =>
            {
                // Set active image for sound.
                soundImage.sprite = isSoundOn ? soundOnImage : soundOffImage;

                // Mute the instruction audio.
                RuntimeConfigurator.Configuration.InstructionPlayer.mute = isSoundOn == false;
            });
        }

        private void SetupLanguagePicker()
        {
            // List of the options to display to user.
            List<string> supportedLanguages = new List<string>();

            // Add each language in capital letters to the list of supported languages.
            foreach (string file in LocalizationFileNames)
            {
                supportedLanguages.Add(file.ToUpper());
            }

            // Setup the dropdown menu.
            languagePicker.AddOptions(supportedLanguages);

            // Set the picker value to the current selected language.
            int languageValue = supportedLanguages.IndexOf(selectedLanguage.ToUpper());
            if (languageValue > -1)
            {
                languagePicker.value = languageValue;
            }
            // If the selected language (system language when starting the scene) has no valid localization file.
            else
            {
                // Either choose the first language of all languages as selected language.
                if (supportedLanguages.Count > 0)
                {
                    selectedLanguage = supportedLanguages[languagePicker.value];
                }
                else if (string.IsNullOrEmpty(LanguageSettings.Instance.DefaultLanguage) == false)
                {
                    languagePicker.AddOptions(new List<string>() { LanguageSettings.Instance.DefaultLanguage.ToUpper() });
                }
                // Or use the fallback language, if there is no valid localization file at all.
                else
                {
                    selectedLanguage = fallbackLanguage;
                    // Add the fallback language as option of the dropdown menu. Otherwise, the picker would be empty.
                    languagePicker.AddOptions(new List<string>() { fallbackLanguage.ToUpper() });
                }
            }

            // When the selected language is changed, setup a training from scratch.
            languagePicker.onValueChanged.AddListener(itemIndex =>
            {
                // Set the supported language based on the user selection.
                selectedLanguage = supportedLanguages[itemIndex];
                LanguageSettings.Instance.ActiveLanguage = selectedLanguage;
                // Load the training and localize it to the selected language.
                SetupTraining();
                // Update the UI.
                SetupTrainingDependantUI();
            });
            
            // If there is only one option, the dropdown is currently disabled.
            SetDropDownStatus(languagePicker);
        }

        private void SetupModePicker()
        {
            // List of the options to display to user.
            List<string> availableModes = new List<string>();

            // Add each mode name to the list of available modes.
            foreach (IMode mode in RuntimeConfigurator.Configuration.Modes.AvailableModes)
            {
                availableModes.Add(mode.Name);
            }
            
            // Setup the dropdown menu.
            modePicker.AddOptions(availableModes);
            
            // Set the picker value to the current selected mode.
            modePicker.value = RuntimeConfigurator.Configuration.Modes.CurrentModeIndex;
            
            // If there is only one option, the dropdown is currently disabled.
            SetDropDownStatus(modePicker);
            
            // When the selected mode is changed,
            modePicker.onValueChanged.AddListener(itemIndex =>
            {
                // Set the mode based on the user selection.
                RuntimeConfigurator.Configuration.Modes.SetMode(itemIndex);
            });
        }
        #endregion

        #region Setup training-dependant UI
        private void SetupTrainingDependantUI()
        {
            SetupChapterPickerOptions();
            SetupTrainingIndicator();
        }

        private void SetupChapterPickerOptions()
        {
            // Show all chapters of the training.
            PopulateChapterPickerOptions(0);
        }

        private void PopulateChapterPickerOptions(int startingIndex)
        {
            // Get a collection of available chapters.
            IList<IChapter> chapters = CourseRunner.Current?.Data.Chapters;

            if (chapters != null)
            {
                // Skip finished chapters and convert the rest to a list of chapter names.
                List<string> dropdownOptions = new List<string>();
                
                for (int i = startingIndex; i < chapters.Count; i++)
                {
                    dropdownOptions.Add(chapters[i].Data.Name);
                }

                // Reset the chapter picker.
                chapterPicker.ClearOptions();

                // Populate it with new options.
                chapterPicker.AddOptions(dropdownOptions);

                // Reset the selected value
                chapterPicker.value = 0;

                // If there is only one option, the dropdown is currently disabled.
                SetDropDownStatus(chapterPicker);
            }
        }

        private void SetupSkipStepPickerOptions()
        {
            // Reset the skip step picker.
            skipStepPicker.ClearOptions();

            if (displayedStep == null)
            {
                return;
            }

            // Get a collection of available transitions (one per target step).
            IList<ITransition> transitions = displayedStep.Data.Transitions.Data.Transitions.ToList();

            // Create a list with all dropdown option names.
            List<string> dropdownOptions = new List<string>();

            if (transitions.Count > 0)
            {
                // Convert the transitions to a list of target step names and use them as dropdown options.
                // null as target step means "end of chapter".
                dropdownOptions = transitions.Select(transition => (transition.Data.TargetStep != null) ? transition.Data.TargetStep.Data.Name : "End of the Chapter").ToList();
            }

            // Populate it with new options.
            skipStepPicker.AddOptions(dropdownOptions);
            skipStepPickerEditorValueField?.SetValue(skipStepPicker, dropdownOptions.Count);
        }

        private void SetupTrainingIndicator()
        {
            if (CourseRunner.Current == null)
            {
                return;
            }
            
            CourseRunner.Current.LifeCycle.StageChanged += (sender, args) =>
            {
                if (args.Stage == Stage.Activating)
                {
                    // Show the indicator when the training is started.
                    trainingStateIndicator.enabled = true;
                }

                if (args.Stage == Stage.Active)
                {
                    // When the training is completed, hide it again.
                    trainingStateIndicator.enabled = false;
                }
            };
        }
        
        private void SetDropDownStatus(TMP_Dropdown dropdown)
        {
            if (dropdown.options.Count <= 1)
            {
                ColorBlock colorBlock = dropdown.colors;
                colorBlock.normalColor = Color.gray;
                dropdown.colors = colorBlock;
                dropdown.enabled = false;
            }
        }
        #endregion
    }
}
