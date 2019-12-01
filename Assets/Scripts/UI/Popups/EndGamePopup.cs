using UnityEngine;
using UnityEngine.UI;

public class EndGamePopup : Popup
{
    #region Constants

    private readonly string[] CONGRADULATIONS = new[]
    {
        "Congradulation!",
        "Cool. Let's try next level.",
        "Awesome!"
    };

    private readonly string[] LOSE_HELPERS = new[]
    {
        "Don't worry! Try again)",
        "Let's try again.",
        "Next time you will success!"
    };

    private const float ONE_STAR_PERCENT = 0.3f;
    private const float TWO_STAR_PERCENT = 0.6f;
    private const float THREE_STAR_PERCENT = 0.9f;

    #endregion
    
    #region Serialized Fields

    [SerializeField] private Image _star1Image, _star2Image, _star3Image;
    [SerializeField] private Button _openLevelsMenuButton;
    [SerializeField] private Button _retryLevelButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Text _congradText;
    [SerializeField] private Color _starActiveColor, _starDeactiveColor;
    [SerializeField] private GameplayPage _gameplayPage;
    [SerializeField] private ChooseLevelPage _chooseLevelPage;

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

    public void Init(int currentPoints, int maximumPoints, bool nextLevelExits, LevelScriptableObj currLevelScriptableObj)
    {
        float percent = (float) currentPoints / (float) maximumPoints;
        SaveStats(percent, currLevelScriptableObj);

        if (percent >= ONE_STAR_PERCENT)
        {
            _star1Image.color = _starActiveColor;
            _congradText.text = CONGRADULATIONS[Random.Range(0, CONGRADULATIONS.Length)];
        }
        else if(percent < ONE_STAR_PERCENT)
        {
            _congradText.text = LOSE_HELPERS[Random.Range(0, LOSE_HELPERS.Length)];
        }

        if (percent >= TWO_STAR_PERCENT)
        {
            _star2Image.color = _starActiveColor;
        }

        if (percent >= THREE_STAR_PERCENT)
        {
            _star3Image.color = _starActiveColor;
        }

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

        _star1Image.color = _star2Image.color = _star3Image.color = _starDeactiveColor;
        _nextLevelButton.gameObject.SetActive(false);
    }

    #endregion

    #endregion
}