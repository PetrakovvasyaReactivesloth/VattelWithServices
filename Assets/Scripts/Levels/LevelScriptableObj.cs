using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[CreateAssetMenu(menuName = "Level")]
public class LevelScriptableObj : ScriptableObject
{
    #region Structs

    public class Word
    {
        public string word;
        public int prizeCoins;
        public string tip;

        public System.Collections.Generic.List<string> synonyms;
        public System.Collections.Generic.List<string> antonyms;
    }

    #endregion

    #region Properties

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public TextAsset LevelJson
    {
        get { return _levelJSON; }
        set { _levelJSON = value; }
    }

    public float LevelTime
    {
        get { return _levelTime; }
        set { _levelTime = value; }
    }

    public int LevelPointsAmount
    {
        get { return _levelPointsAmount; }
        set { _levelPointsAmount = value; }
    }

    public List<Word> WordsInLevel
    {
        get { return _wordsInLevel; }
        set { _wordsInLevel = value; }
    }

    #endregion

    #region Serialized Fields

    [SerializeField] private TextAsset _levelJSON;

    #endregion

    #region Private Fields

    private float _levelTime;
    private int _levelPointsAmount;
    private List<Word> _wordsInLevel = new List<Word>();
    private string _title;

    #endregion

    #region Methods

    #region Public Methods

    public void Parse()
    {
        Reset();
        
        JSONNode level = JSON.Parse(LevelJson.text);
        LevelTime = (float) level["levelTime"];
        LevelPointsAmount = (int) level["levelPointsAmount"];
        Title = level["levelName"];

        JSONArray words = (JSONArray) level["words"];

        foreach (JSONNode word in words)
        {
            List<string> synonymsList = new List<string>();
            JSONArray synonymsArr = (JSONArray) word["synonyms"];
            foreach (JSONNode synonym in synonymsArr)
            {
                if (!synonymsList.Contains(synonym))
                {
                    synonymsList.Add(synonym);
                }
            }

            List<string> antonymsList = new List<string>();
            JSONArray antonymsArr = (JSONArray) word["antonyms"];
            foreach (JSONNode antonym in antonymsArr)
            {
                if (!antonymsList.Contains(antonym))
                {
                    antonymsList.Add(antonym);
                }
            }

            var newWord = new Word()
            {
                word = word["word"],
                prizeCoins = word["prizeCoins"],
                tip = word["tip"],
                synonyms = synonymsList,
                antonyms = antonymsList
            };

            if (!WordsInLevel.Contains(newWord))
            {
                WordsInLevel.Add(newWord);
            }
        }
    }

    #endregion

    #region Private Methods

    private void Reset()
    {
        LevelTime = 0;
        LevelPointsAmount = 0;
        WordsInLevel.Clear();
    }

    #endregion

    #endregion

    public void DebugInfo()
    {
        Debug.Log("Время на уровень (сек): " + LevelTime);
        Debug.Log("Необходимо набрать очков на уровень: " + LevelPointsAmount);
        Debug.Log("Слова: ");

        foreach (var word in WordsInLevel)
        {
            Debug.Log("Слово: " + word.word);
            Debug.Log("Подсказка: " + word.tip);
            Debug.Log("Очков за правильный ответ: " + word.prizeCoins);
            Debug.Log("Синонимы: ");

            foreach (var wordSynonym in word.synonyms)
            {
                Debug.Log(wordSynonym);
            }
            
            Debug.Log("Антонимы: ");
            foreach (var wordAntonym in word.antonyms)
            {
                Debug.Log(wordAntonym);
            }
        }
    }
}