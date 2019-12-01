using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseDifficultyPage : Page
{
    #region Serialized Fields

    [SerializeField] private MainMenuPage _mainMenuPage;
    [SerializeField] private GameBuilder _gameBuilder;
    [SerializeField] private DifficultyLevelButton _difficultyLevelButtonPrefab;
    [SerializeField] private Transform _difficultyLevelButtonsParenTransform;
    [SerializeField] private Button _backButton;
    [SerializeField] private ChooseLevelPage _chooseLevelPage;

    #endregion

    #region Private Fields

    private Dictionary<LanguageWordsDictionary.Difficulty, DifficultyLevelButton> _instantiatedDifficulties = new Dictionary<LanguageWordsDictionary.Difficulty, DifficultyLevelButton>();

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _backButton.onClick.AddListener(() =>
        {
            Hide();
            _mainMenuPage.Show();
        });
    }

    #endregion

    #region Override Methods

    public override void Show()
    {
        base.Show();

        Init(_gameBuilder.LanguageWordsDictionary);
    }

    private void Init(LanguageWordsDictionary gameBuilderLanguageWordsDictionary)
    {
        foreach (var difficultyLevel in gameBuilderLanguageWordsDictionary._difficultyLevels)
        {
            if (!_instantiatedDifficulties.ContainsKey(difficultyLevel))
            {
                var diffLevel = Instantiate(_difficultyLevelButtonPrefab, _difficultyLevelButtonsParenTransform);
                
                diffLevel.Init(difficultyLevel, _chooseLevelPage, this);
                _instantiatedDifficulties.Add(difficultyLevel, diffLevel);
            }
            else
            {
                _instantiatedDifficulties[difficultyLevel].RefreshStarsFilling();
            }
        }
    }

    #endregion

    #endregion
}