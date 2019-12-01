using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : Page
{
    #region Serialized Fields

    [SerializeField] private Button _openSettingsPageButton;
    [SerializeField] private Button _openRecordsPageButton;
    [SerializeField] private SettingsPopup _settingsPopup;
    [SerializeField] private RecordsMenuPage _recordsMenuPage;
    [SerializeField] private Button _playButton;
    [SerializeField] private ChooseDifficultyPage _chooseDifficultyPage;
    [SerializeField] private GameObject _mainPanelGameObject;
    [SerializeField] private GameObject _loadingBundlesPanelGameObject;


    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _openSettingsPageButton.onClick.AddListener(() =>
        {
            _settingsPopup.Show();
            Hide();
        });

        _openRecordsPageButton.onClick.AddListener(() =>
        {
            _recordsMenuPage.Show();
            Hide();
        });

        _playButton.onClick.AddListener(() =>
        {
            _chooseDifficultyPage.Show();
            Hide();
        });

        AssetBundlesLoader.Instance.RegisterOnLevelsDictionaryLoadListener(OnLevelsDictionaryLoaded);
    }

    private void OnLevelsDictionaryLoaded()
    {
        _loadingBundlesPanelGameObject.SetActive(false);
        _mainPanelGameObject.SetActive(true);
    }

    #endregion

    #endregion
}