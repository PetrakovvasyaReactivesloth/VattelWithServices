using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyLevelButton : MonoBehaviour
{
    #region Cosntants

    private const string SCORE_TEXT = "";

    #endregion

    #region Serialized Fields

    [SerializeField] private Text _titleText;
    [SerializeField] private Image _starsProgresBarImage;
    [SerializeField] private Text _scoreText;

    #endregion

    #region Private Fields

    private List<LevelScriptableObj> _levelScriptableObjs = new List<LevelScriptableObj>();
    private Button _cachedButton;
    private ChooseLevelPage _chooseLevelPage;
    private ChooseDifficultyPage _chooseDifficultyPage;
    private AudioSource _cachedAudioSource;
    private string _leaderBoardsTableID;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _cachedButton = GetComponent<Button>();
        _cachedAudioSource = GetComponent<AudioSource>();

        _cachedButton.onClick.AddListener(() =>
        {
            _cachedAudioSource.Play();

            _chooseLevelPage.Init(_levelScriptableObjs, _leaderBoardsTableID);
            _chooseLevelPage.Show();
            _chooseDifficultyPage.Invoke("Hide", _cachedAudioSource.clip.length);
        });
    }

    #endregion

    #region Public Methods

    public void Init(LanguageWordsDictionary.Difficulty difficultyLevel, ChooseLevelPage chooseLevelPage, ChooseDifficultyPage chooseDifficultyPage)
    {
        _chooseLevelPage = chooseLevelPage;
        _chooseDifficultyPage = chooseDifficultyPage;
        _titleText.text = difficultyLevel.title;
        _levelScriptableObjs = difficultyLevel._levels;
        _leaderBoardsTableID = difficultyLevel.leaderBoardsTableID;

        RefreshStarsFilling();
    }

    #endregion

    #endregion

    public void RefreshStarsFilling()
    {
        float completedLevelProgresesPercentSum = 0;
        float notNormalizedPointsSum = 0;

        foreach (var levelScriptableObj in _levelScriptableObjs)
        {
            levelScriptableObj.Parse();
            completedLevelProgresesPercentSum += PlayerPrefsManager.GetNormalizedPercent(PlayerPrefsManager.GetSavedLevelStats(levelScriptableObj));
            float savedStats = PlayerPrefsManager.GetSavedLevelStats(levelScriptableObj);
            float pointsAmount = levelScriptableObj.LevelPointsAmount;
            notNormalizedPointsSum += savedStats * pointsAmount;
            levelScriptableObj.Parse();
        }

        float difficultyCompletePercent = completedLevelProgresesPercentSum / _levelScriptableObjs.Count;
        _starsProgresBarImage.fillAmount = difficultyCompletePercent;

        _scoreText.text = SCORE_TEXT + notNormalizedPointsSum;
    }
}