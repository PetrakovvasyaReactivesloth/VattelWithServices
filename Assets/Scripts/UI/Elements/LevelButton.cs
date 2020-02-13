using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Text _titleText;
    [SerializeField] private Image _starsProgresBarImage;

    #endregion

    #region Private Fields

    private GameplayPage _gameplayPage;
    private Button _cachedButton;
    private LevelScriptableObj _currentLevelScriptableObj;
    private ChooseLevelPage _chooseLevelPage;
    private AudioSource _cachedAudioSource;
    private string _leaderboardsTableID;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _cachedAudioSource = GetComponent<AudioSource>();
        _cachedButton = GetComponent<Button>();
        _cachedButton.onClick.AddListener(() =>
        {
            _cachedAudioSource.Play();
            _gameplayPage.Show();

            _gameplayPage.Init(_currentLevelScriptableObj, _leaderboardsTableID, _chooseLevelPage.GetNextLevelAfterCurrent(_currentLevelScriptableObj));
            _chooseLevelPage.Invoke("Hide", _cachedAudioSource.clip.length);
        });
    }

    #endregion

    #region Public Methods

    public void Init(LevelScriptableObj levelScriptableObj, GameplayPage gameplayPage, ChooseLevelPage chooseLevelPage, string leaderboardsTableID)
    {
        _currentLevelScriptableObj = levelScriptableObj;
        _chooseLevelPage = chooseLevelPage;
        _titleText.text = _currentLevelScriptableObj.Title;
        gameObject.name = _currentLevelScriptableObj.Title + this.ToString();
        _gameplayPage = gameplayPage;

        float savedProgress =  PlayerPrefsManager.GetNormalizedPercent(PlayerPrefsManager.GetSavedLevelStats(levelScriptableObj));
        _starsProgresBarImage.fillAmount = savedProgress;
        _leaderboardsTableID = leaderboardsTableID;
    }

    #endregion

    #endregion
}
