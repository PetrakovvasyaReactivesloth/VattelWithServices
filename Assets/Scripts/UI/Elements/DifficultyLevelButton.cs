using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyLevelButton : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Text _titleText;
    [SerializeField] private Image _starsProgresBarImage;

    #endregion

    #region Private Fields

    private List<LevelScriptableObj> _levelScriptableObjs = new List<LevelScriptableObj>();
    private Button _cachedButton;
    private ChooseLevelPage _chooseLevelPage;
    private ChooseDifficultyPage _chooseDifficultyPage;
    private AudioSource _cachedAudioSource;

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

            _chooseLevelPage.Init(_levelScriptableObjs);
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
        RefreshStarsFilling();
    }

    #endregion

    #endregion

    public void RefreshStarsFilling()
    {
        float completedLevelProgresesPercentSum = 0;
        foreach (var levelScriptableObj in _levelScriptableObjs)
        {
            completedLevelProgresesPercentSum += PlayerPrefsManager.GetSavedLevelStats(levelScriptableObj);
        }

        float difficultyCompletePercent = completedLevelProgresesPercentSum / _levelScriptableObjs.Count;
        _starsProgresBarImage.fillAmount = difficultyCompletePercent;
    }
}