using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    #region Constants

    private const string PERCENT_STRING = "%";

    #endregion

    #region Serialized Fields

    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Text _musicVolumeSliderValueText;
    [SerializeField] private Slider _soundEffectsVolumeSlider;
    [SerializeField] private Text _soundEffectsVolumeSliderValueText;
    [SerializeField] private Button _backToMainMenuButton;
    [SerializeField] private MainMenuPage _mainMenuPage;
    [SerializeField] private SettingsManager _settingsManager;
    [SerializeField] private Button _resetProgressButton;
    [SerializeField] private Transform _backgroundsContentParentTransform;
    [SerializeField] private BackgroundChangerButton _backgroundChangerButtonPrefab;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _backToMainMenuButton.onClick.AddListener(() =>
        {
            Hide();
            _mainMenuPage.Show();
            PlayerPrefsManager.SaveMusicVolume(_musicVolumeSlider);
            PlayerPrefsManager.SaveSoundEffectsVolume(_soundEffectsVolumeSlider);
        });

        
        _musicVolumeSlider.onValueChanged.AddListener((value =>
        {
            SetVolumeText(_musicVolumeSliderValueText, _musicVolumeSlider);
            _settingsManager.SetMusicVolume(value);
        }));

        _soundEffectsVolumeSlider.onValueChanged.AddListener((value =>
        {
            SetVolumeText(_soundEffectsVolumeSliderValueText, _soundEffectsVolumeSlider);
            _settingsManager.SetSoundEffectsVolume(value);
        }));

        _resetProgressButton.onClick.AddListener(() =>
        {
            Debug.Log("Возможно после нажатия reset progress стоить отображать окошко, уверены ли вы");

            PlayerPrefsManager.ResetProgress();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Hide();
            _mainMenuPage.Show();
        });
    }

    private void Start()
    {
        SetSavedVolumeValuesToSliders();
        SetVolumeText(_musicVolumeSliderValueText, _musicVolumeSlider);
        SetVolumeText(_soundEffectsVolumeSliderValueText, _soundEffectsVolumeSlider);

        foreach (var backgroundSprite in BackgroundManager.Instance.BackgroundSprites)
        {
            var instantiatedBackgroundButton =
                Instantiate(_backgroundChangerButtonPrefab, _backgroundsContentParentTransform);
            instantiatedBackgroundButton.Init(backgroundSprite);
        }
    }

    #endregion

    #region Private Methods

    private void SetSavedVolumeValuesToSliders()
    {
        _musicVolumeSlider.value = _settingsManager.GetSavedMusicVolumeValue();
        _soundEffectsVolumeSlider.value = _settingsManager.GetSavedSoundEffectsValue();
    }

    private void SetVolumeText(Text volText, Slider valueSlider)
    {
        float value = valueSlider.value;
        volText.text = (int) (value * 100) + PERCENT_STRING;
    }

    #endregion

    #endregion
}