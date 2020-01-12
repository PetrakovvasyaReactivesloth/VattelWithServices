using UnityEngine;
using UnityEngine.UI;

public class EndGamePopup : Popup
{
    #region Constants

    private readonly string[] CONGRADULATIONS = new[]
    {
        "Congratulations!",
        "Cool. Let's try next level.",
        "Awesome!"
    };

    private readonly string[] LOSE_HELPERS = new[]
    {
        "Don't worry! Try again.",
        "Let's try again.",
        "Next time you will success!"
    };

    private const float ONE_STAR_PERCENT = 0.3f;
    private const float TWO_STAR_PERCENT = 0.6f;
    private const float THREE_STAR_PERCENT = 0.9f;
    private const float COMPLETED_LEVEL_PERCENT = 1f;
    private const int ONE_HUNDERT_PERCENT = 100;

    #endregion
    
    #region Serialized Fields

    [SerializeField] private Image _stars;
    [SerializeField] private Button _openLevelsMenuButton;
    [SerializeField] private Button _retryLevelButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Text _congradText;
    [SerializeField] private Color _starActiveColor, _starDeactiveColor;
    [SerializeField] private GameplayPage _gameplayPage;
    [SerializeField] private ChooseLevelPage _chooseLevelPage;
    [SerializeField] private LeaderboardsManager _leaderboardsManager;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _retryLevelButton.onClick.AddListener(() =>
        {
            _gameplayPage.RestartLevel();
            Hide();
        });

        _openLevelsMenuButton.onClick.AddListener(() =>
        {
            _chooseLevelPage.Show();
            _chooseLevelPage.ShowCurrentLevelsList();

            Hide();
        });

        _nextLevelButton.onClick.AddListener(() =>
        {
            _gameplayPage.StartNextLevel();
            Hide();
        });
    }

    #endregion

    #region Public Methods

    public void Init(int currentPoints, int maximumPoints, string leaderboardsTableID, bool nextLevelExits, LevelScriptableObj currLevelScriptableObj)
    {
        float percent = (float)currentPoints / (float)maximumPoints;
        //PlayerPrefsManager.GetNormalizedPercent(percent);

        SaveStats(percent, currLevelScriptableObj);
        var _levelScriptableObjs = _chooseLevelPage.CurrentLevelScriptableObjs;
        float completedLevelProgresesPercentSum = 0;
        foreach (var levelScriptableObj in _levelScriptableObjs)
        {
            var pointsPerLevel = (levelScriptableObj.LevelPointsAmount * PlayerPrefsManager.GetSavedLevelStats(levelScriptableObj));

            completedLevelProgresesPercentSum +=  pointsPerLevel;
        }

        _leaderboardsManager.PostScores((int) completedLevelProgresesPercentSum, leaderboardsTableID);

        if (percent >= ONE_STAR_PERCENT)
        {
            _congradText.text = CONGRADULATIONS[Random.Range(0, CONGRADULATIONS.Length)].ToUpper();
        }
        else if (percent < ONE_STAR_PERCENT)
        {
            _congradText.text = LOSE_HELPERS[Random.Range(0, LOSE_HELPERS.Length)].ToUpper();
        }

        _stars.fillAmount = percent;
        
        if (nextLevelExits)
        {
            _nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            _nextLevelButton.gameObject.SetActive(false);
        }
    }

    private void SaveStats(float percent, LevelScriptableObj currLevelScriptableObj)
    {
        if (PlayerPrefsManager.GetSavedLevelStats(currLevelScriptableObj) < percent)
        {
            PlayerPrefsManager.SaveStats(percent, currLevelScriptableObj);
        }
    }

    #endregion

    #region Override Methods

    public override void Hide()
    {
        base.Hide();

        _stars.fillAmount = 0;
        _nextLevelButton.gameObject.SetActive(false);
    }

    #endregion

    #endregion
}