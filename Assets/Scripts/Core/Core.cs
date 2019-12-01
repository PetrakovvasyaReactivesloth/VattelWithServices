using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private TextAsset words, synonyms, antonyms;
    [SerializeField] private Text wordText, synonymAntonymText;
    [SerializeField] private Image answerSuccessOrWrongIndicatorImage;
    [SerializeField] private Sprite _answerSuccessSprite, _answerWrongSprite;
    [SerializeField] private Button[] buttons;

    #endregion

    #region Private Fields

    private bool isSynonymOrAntonymSecondWord;
    private int previousWordIndex;
    private string[] wordsArray, synonymsArray, antonymsArray;
    private Menu menu;

    #endregion

    #region Methods

    #region Unity Methods

    private void Start()
    {
        menu = FindObjectOfType<Menu>();

        wordsArray = words.text.Split('\n');
        synonymsArray = synonyms.text.Split('\n');
        antonymsArray = antonyms.text.Split('\n');

        DrawLevel();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Ставит текущее слово и то слово, с которым сравнивается текущее
    /// </summary>
    private void DrawLevel()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        int currentWordIndex = Random.Range(0, wordsArray.Length - 1);

        if (currentWordIndex != previousWordIndex)
        {
            if (Random.Range(0.0f, 10.0f) < 5.0f)
            {
                isSynonymOrAntonymSecondWord = true;
            }
            else
            {
                isSynonymOrAntonymSecondWord = false;
            }

            wordText.text = wordsArray[currentWordIndex];
            synonymAntonymText.text = isSynonymOrAntonymSecondWord
                ? synonymsArray[currentWordIndex]
                : antonymsArray[currentWordIndex]; //Ставим слово, с которым сравниваем текущее

            previousWordIndex = currentWordIndex;
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
        answerSuccessOrWrongIndicatorImage.gameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Вызывается из Inspector в обработчике кнопки Synonym
    /// </summary>
    public void ThisIsSynonym()
    {
        CancelInvoke();

        answerSuccessOrWrongIndicatorImage.gameObject.SetActive(true);
        answerSuccessOrWrongIndicatorImage.sprite =
            isSynonymOrAntonymSecondWord ? _answerSuccessSprite : _answerWrongSprite;

        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        menu.PlaySound(false);
        DrawLevel();
        Invoke("FadeAnswer", 1);
    }

    /// <summary>
    /// Вызывается из Inspector в обработчике кнопки Antonym
    /// </summary>
    public void ThisIsAntonym()
    {
        CancelInvoke();

        answerSuccessOrWrongIndicatorImage.gameObject.SetActive(true);
        answerSuccessOrWrongIndicatorImage.sprite =
            isSynonymOrAntonymSecondWord ? _answerWrongSprite : _answerSuccessSprite;

        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        menu.PlaySound(false);
        DrawLevel();
        Invoke("FadeAnswer", 1);
    }

    public void BackToLevelsMenu()
    {
        menu.PlaySound(true);
        menu.OpenMenuScreen();
    }

    #endregion

    #region Static Methods

    private static string Translit(string str)
    {
        string[] lat_up =
        {
            "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U",
            "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya"
        };
        string[] lat_low =
        {
            "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u",
            "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya"
        };
        string[] rus_up =
        {
            "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У",
            "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"
        };
        string[] rus_low =
        {
            "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у",
            "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"
        };
        for (int i = 0; i <= 32; i++)
        {
            str = str.Replace(rus_up[i], lat_up[i]);
            str = str.Replace(rus_low[i], lat_low[i]);
        }

        return str;
    }

    #endregion

    #endregion
}