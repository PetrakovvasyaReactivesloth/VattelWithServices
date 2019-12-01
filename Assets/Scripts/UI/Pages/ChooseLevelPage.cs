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
    [SerializeField] private Button _leaderBoardsButton;
    [SerializeField] private LeaderboardsManager _leaderboardsManager;
    [SerializeField] private AuthManager _authManager;

    #endregion

    #region Private Fields

    private List<LevelScriptableObj> _currentLevelScriptableObjs;
    private string _leaderBoardsTableID;

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

        _leaderBoardsButton.onClick.AddListener(() =>
        {
            _leaderboardsManager.ShowLeaderboard(_leaderBoardsTableID);
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
        Init(_currentLevelScriptableObjs, _leaderBoardsTableID);
    }

    public void Init(List<LevelScriptableObj> levelScriptableObjs, string leaderboardTableID)
    {
        RemoveAllElements();

        _leaderBoardsTableID = leaderboardTableID;
        _currentLevelScriptableObjs = levelScriptableObjs;

        foreach (var levelScriptableObj in levelScriptableObjs)
        {
            var but = Instantiate(_levelButtonPrefab, _levelButtonsParenTransform);

            levelScriptableObj.Parse();
            but.Init(levelScriptableObj, _gameplayPage, this, leaderboardTableID);
        }

        if (_authManager.CheckAuth())
        {
            _leaderBoardsButton.gameObject.SetActive(true);
        }
        else
        {
            _leaderBoardsButton.gameObject.SetActive(false);
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