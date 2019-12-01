using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPage : Page
{
    #region Constants

    private const float FADE_ANSWER_STATUS_DELAY_SECONDS = 1f;
    private const float ONE_SECOND = 1f;
    private readonly Color CORRECT_ANSWER_COLOR = Color.green;
    private readonly Color WRONG_ANSWER_COLOR = Color.red;


    #endregion

    #region Properties

    private int CurrentPoints
    {
        get { return _currentPoints; }
        set
        {
            _currentPoints = value;

            if (_currentPoints >= _levelMaximumPoints)
            {
                _currentPoints = _levelMaximumPoints;
                ShowEndGamePanelWithStats();
            }

            if (_currentPoints <= 0)
            {
                _currentPoints = 0;
            }

            _levelPointsText.text = _currentPoints + "/" + _levelMaximumPoints;
        }
    }

    #endregion

    #region Serialized Field

    [SerializeField] private Text _mainWordText;
    [SerializeField] private Text _compareWordText;
    [SerializeField] private Button _synonymButton;
    [SerializeField] private Button _antonymButton;
    [SerializeField] private Button _tipButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image answerSuccessOrWrongIndicatorImage;
    [SerializeField] private Sprite _answerSuccessSprite, _answerWrongSprite;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _levelPointsText;
    [SerializeField] private EndGamePopup _endGamePopup;
    [SerializeField] private ChooseLevelPage _chooseLevelPage;
    [SerializeField] private TipPopup _tipPopup;

    #endregion

    #region Private Fields

    private LevelScriptableObj _currentLevelScriptableObj;
    private LevelScriptableObj _nextLevelScriptableObj;
    private bool isSynonymSecondWord;
    private int previousWordIndex;
    private float _currentTime;
    private int _currentPoints;
    private int _levelMaximumPoints;
    private bool _gameEnd;

    #endregion

    #region Methods

    #region Methods

    private void Awake()
    {
        _synonymButton.onClick.AddListener(() => { ThisIsSynonym(); });

        _antonymButton.onClick.AddListener(() => { ThisIsAntonym(); });

        _tipButton.onClick.AddListener(() =>
        {
            _tipPopup.Show();
            _tipPopup.Init(GetCurrentWord());
        });

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            _chooseLevelPage.Show();
        });
    }

    #endregion

    #region Public Methods

    public void RestartLevel()
    {
        Init(_currentLevelScriptableObj);
    }

    public void StartNextLevel()
    {
        Init(_nextLevelScriptableObj, _chooseLevelPage.GetNextLevelAfterCurrent(_nextLevelScriptableObj));
    }

    public void Init(LevelScriptableObj levelScriptableObj, LevelScriptableObj nextLevelScriptableObj = null)
    {
        _nextLevelScriptableObj = null;
        _currentLevelScriptableObj = null;
        _tipPopup.Hide();
        _gameEnd = false;
        EnableAllElements();
        _currentLevelScriptableObj = levelScriptableObj;
        _nextLevelScriptableObj = nextLevelScriptableObj;
        //_currentLevelScriptableObj.Parse();
        DrawLevel();
        SetLevelMaximumTime(_currentLevelScriptableObj.LevelTime);
        SetLevelMaximumPoints(_currentLevelScriptableObj.LevelPointsAmount);
    }

    #endregion

    #region Private Methods

    private void ShowEndGamePanelWithStats()
    {
        _endGamePopup.Show();
        _gameEnd = true;
        _tipPopup.Hide();
        _endGamePopup.Init(_currentPoints, _levelMaximumPoints, _nextLevelScriptableObj != null,
            _currentLevelScriptableObj);
        StopAllCoroutines();
        DisableAllElements();
    }

    private void DisableAllElements()
    {
        _tipButton.gameObject.SetActive(false);
        _antonymButton.gameObject.SetActive(false);
        _synonymButton.gameObject.SetActive(false);
        _closeButton.gameObject.SetActive(false);
        _timerText.gameObject.SetActive(false);
        _levelPointsText.gameObject.SetActive(false);
        _compareWordText.gameObject.SetActive(false);
        _mainWordText.gameObject.SetActive(false);
    }

    private void EnableAllElements()
    {
        if (!_gameEnd)
        {
            _tipButton.gameObject.SetActive(true);
            _antonymButton.gameObject.SetActive(true);
            _synonymButton.gameObject.SetActive(true);
            _closeButton.gameObject.SetActive(true);
            _timerText.gameObject.SetActive(true);
            _levelPointsText.gameObject.SetActive(true);
            _compareWordText.gameObject.SetActive(true);
            _mainWordText.gameObject.SetActive(true);
        }
    }

    private IEnumerator TimerWork()
    {
        while (_currentTime >= 0)
        {
            string minutes = Mathf.Floor(_currentTime / 60).ToString("00");
            string seconds = (_currentTime % 60).ToString("00");

            _timerText.text = string.Format("{0}:{1}", minutes, seconds);

            yield return new WaitForSeconds(ONE_SECOND);
            _currentTime -= ONE_SECOND;
        }

        ShowEndGamePanelWithStats();
    }

    private void SetLevelMaximumPoints(int levelPointsAmount)
    {
        _levelMaximumPoints = levelPointsAmount;
        CurrentPoints = 0;
    }

    private void SetLevelMaximumTime(float levelTime)
    {
        _currentTime = levelTime;
        StartCoroutine(TimerWork());
    }

    /// <summary>
    /// Ставит текущее слово и то слово, с которым сравнивается текущее
    /// </summary>
    private void DrawLevel()
    {
        int currentWordIndex = Random.Range(0, _currentLevelScriptableObj.WordsInLevel.Count);

        if (currentWordIndex != previousWordIndex)
        {
            if (Random.Range(0.0f, 10.0f) < 5.0f)
            {
                isSynonymSecondWord = true;
            }
            else
            {
                isSynonymSecondWord = false;
            }

            previousWordIndex = currentWordIndex;

            var currentWord = GetCurrentWord();
            _mainWordText.text = currentWord.word.ToUpper();

            int randomSynonymIndex =
                Random.Range(0, currentWord.synonyms.Count);
            int randomAntonymIndex =
                Random.Range(0, currentWord.antonyms.Count);

            _compareWordText.text = isSynonymSecondWord
                ? currentWord.synonyms[randomSynonymIndex].ToUpper()
                : currentWord.antonyms[randomAntonymIndex].ToUpper(); //Ставим слово, с которым сравниваем текущее
        }
        else //Снова выбираем рандомное слово
        {
            DrawLevel();
        }
    }

    /// <summary>
    /// Вызывается из Invoke, прячет галочку или крестик подтверждающие правильность вввода
    /// </summary>
    private void FadeAnswer()
    {
        DrawLevel();
        EnableSynonymAntonymButtons();
        answerSuccessOrWrongIndicatorImage.gameObject.SetActive(false);
    }

    private void EnableSynonymAntonymButtons()
    {
        if (!_gameEnd)
        {
            _synonymButton.gameObject.SetActive(true);
            _antonymButton.gameObject.SetActive(true);
        }
    }

    private void DisableSynonymAntonymButtons()
    {
        _synonymButton.gameObject.SetActive(false);
        _antonymButton.gameObject.SetActive(false);
    }

    private void ThisIsSynonym()
    {
        CancelInvoke();

        _tipPopup.Hide();
        answerSuccessOrWrongIndicatorImage.gameObject.SetActive(true);
        answerSuccessOrWrongIndicatorImage.sprite =
            isSynonymSecondWord ? _answerSuccessSprite : _answerWrongSprite;
        answerSuccessOrWrongIndicatorImage.color = isSynonymSecondWord ? CORRECT_ANSWER_COLOR : WRONG_ANSWER_COLOR;


        int p = GetCurrentWord().prizeCoins;

        if (isSynonymSecondWord)
        {
            CurrentPoints += p;
        }
        else
        {
            CurrentPoints -= p;
        }

        Invoke("FadeAnswer", 1);
        DisableSynonymAntonymButtons();
    }

    private void ThisIsAntonym()
    {
        CancelInvoke();

        _tipPopup.Hide();
        answerSuccessOrWrongIndicatorImage.gameObject.SetActive(true);
        answerSuccessOrWrongIndicatorImage.sprite =
            isSynonymSecondWord ? _answerWrongSprite : _answerSuccessSprite;
        answerSuccessOrWrongIndicatorImage.color = isSynonymSecondWord ? WRONG_ANSWER_COLOR : CORRECT_ANSWER_COLOR;

        int p = GetCurrentWord().prizeCoins;

        if (!isSynonymSecondWord)
        {
            CurrentPoints += p;
        }
        else
        {
            CurrentPoints -= p;
        }

        Invoke("FadeAnswer", 1);
        DisableSynonymAntonymButtons();
    }

    private LevelScriptableObj.Word GetCurrentWord()
    {
        return _currentLevelScriptableObj.WordsInLevel[previousWordIndex];
    }

    #endregion

    #region Override Methods

    public override void Hide()
    {
        base.Hide();

        StopAllCoroutines();
        _nextLevelScriptableObj = null;
        _currentLevelScriptableObj = null;
        _tipPopup.Hide();
    }

    #endregion

    #endregion
}