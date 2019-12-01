using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelPage : Page
{
    #region Serialized Fields

    [SerializeField] private LevelButton _levelButtonPrefab;
    [SerializeField] private Transform _levelButtonsParenTransform;
    [SerializeField] private Button _backButton;
    [SerializeField] private GameplayPage _gameplayPage;
    [SerializeField] private ChooseDifficultyPage _chooseDifficultyPage;

    #endregion

    #region Private Fields

    private List<LevelScriptableObj> _currentLevelScriptableObjs;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _backButton.onClick.AddListener(() =>
        {
            Hide();
            _chooseDifficultyPage.Show();
        });
    }

    #endregion

    #region Public Methods

    public LevelScriptableObj GetNextLevelAfterCurrent(LevelScriptableObj current)
    {
        int currentIndex =
            _currentLevelScriptableObjs.IndexOf(_currentLevelScriptableObjs.FirstOrDefault(c => c == current));

        LevelScriptableObj temp = (currentIndex == _currentLevelScriptableObjs.Count - 1)
            ? null
            : _currentLevelScriptableObjs[currentIndex + 1];

        return temp;
    }

    public void ShowCurrentLevelsList()
    {
        Init(_currentLevelScriptableObjs);
    }

    public void Init(List<LevelScriptableObj> levelScriptableObjs)
    {
        RemoveAllElements();

        _currentLevelScriptableObjs = levelScriptableObjs;

        foreach (var levelScriptableObj in levelScriptableObjs)
        {
            var but = Instantiate(_levelButtonPrefab, _levelButtonsParenTransform);

            levelScriptableObj.Parse();
            but.Init(levelScriptableObj, _gameplayPage, this);
        }
    }

    #endregion

    #region Private Methods

    private void RemoveAllElements()
    {
        foreach (Transform child in _levelButtonsParenTransform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    #endregion
}