using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : Page
{
    #region Serialized Fields

    [SerializeField] private Button _openSettingsPageButton;
    [SerializeField] private Button _openRecordsPageButton;
    [SerializeField] private SettingsPopup _settingsPopup;
    [SerializeField] private Button _playButton;
    [SerializeField] private ChooseDifficultyPage _chooseDifficultyPage;
    [SerializeField] private GameObject _mainPanelGameObject;
    [SerializeField] private GameObject _loadingBundlesPanelGameObject;
    [SerializeField] private LeaderboardsManager _leaderboardsManager;
    [SerializeField] private AuthManager _authManager;

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
            _leaderboardsManager.ShowAllLeaderboards();
        });

        _playButton.onClick.AddListener(() =>
        {
            _chooseDifficultyPage.Show();
            Hide();
        });

        WWWLevelsLoader.Instance.RegisterOnLevelsDictionaryLoadListener(OnLevelsDictionaryLoaded);
    }

    private void OnLevelsDictionaryLoaded()
    {
        _loadingBundlesPanelGameObject.SetActive(false);
        _mainPanelGameObject.SetActive(true);
        Show();
    }

    #endregion

    #region Override Methods

    public override void Show()
    {
        base.Show();
        
        CheckAuth();
    }

    private void CheckAuth()
    {
        if (_authManager.CheckAuth())
        {
            //_openRecordsPageButton.gameObject.SetActive(true);
        }
        else
        {
            //_openRecordsPageButton.gameObject.SetActive(false);
        }
    }

    #endregion

    #endregion
}