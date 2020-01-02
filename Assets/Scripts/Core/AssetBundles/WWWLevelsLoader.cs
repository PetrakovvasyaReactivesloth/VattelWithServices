using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class WWWLevelsLoader : MonoBehaviour
{
    #region Properties

    public static WWWLevelsLoader Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                _instance = new GameObject("WWWLevelsLoader").AddComponent<WWWLevelsLoader>();
            }

            return _instance; 
        }

        set => _instance = value; 
    }

    public LanguageWordsDictionary LoadedLevelsDictionary { get => _loadedLevelsDictionary; set => _loadedLevelsDictionary = value; }

    #endregion

    #region Constants

    private const string BASE_URL = "https://reactivesloth.com/vpetrakov/vattel-english/WWWLoad/";
    private const string SLASH_STRING = "/";
    private const string LANGUAGES_AVAIBLE_FILE_TITLE = "AvaibleLanguages.txt";
    private const string DIFFICULTIES_AVAIBLE_FILE_TITLE = "AvaibleDifficultiesWithGoogleTablesID.txt";
    private const string LEVELS_AVAIBLE_FILE_TITLE = "AvaibleLevels.txt";
    private const string LEVELS_FOLDER_TITLE = "Levels";

    #endregion

    #region Private Fields

    private SystemLanguage _currentLanguage = SystemLanguage.English;//На сервере есть файл AvaibleLanguages.txt, в нем должны быть ToSting значения системных языков
    private LanguageWordsDictionary _loadedLevelsDictionary;
    private Dictionary<string, List<LevelScriptableObj>> _difficultiesLevelsDictionary = new Dictionary<string, List<LevelScriptableObj>>();
    private static WWWLevelsLoader _instance = null;
    private List<System.Action> _levelsDictionaryLoadedListeners = new List<System.Action>();
    
    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadedLevelsDictionary = new LanguageWordsDictionary();

        StartCoroutine(LoadAvaibleLanguages((avaible_lanuages_string) =>
        {
            string[] avaible_languages_arr = RemoveEmptyLines(avaible_lanuages_string.Split('\n'));
            string currentChosedLanguageKey = string.Empty;
            int loadedDifficultiesCount = 0;

            foreach (var languageFromServerKey in avaible_languages_arr)
            {
                if(languageFromServerKey == _currentLanguage.ToString())
                {
                    currentChosedLanguageKey = languageFromServerKey;
                    LoadedLevelsDictionary._title = languageFromServerKey;

                    break;
                }
            }

            StartCoroutine(LoadAvaibleDifficulties(currentChosedLanguageKey, (avaible_difficulties_string) =>
            {
                string[] avaible_difficulties_arr = RemoveEmptyLines(avaible_difficulties_string.Split('\n'));

                foreach (var difficultyFromServerKey in avaible_difficulties_arr)
                {
                    string difficultyString = difficultyFromServerKey.Split('=')[0];
                    string googleTableID = difficultyFromServerKey.Split('=')[1];

                    if(!_difficultiesLevelsDictionary.ContainsKey(difficultyString))
                    {
                        _difficultiesLevelsDictionary.Add(difficultyString, new List<LevelScriptableObj>());
                    }

                    StartCoroutine(LoadAvaibleLevels(currentChosedLanguageKey, difficultyString, googleTableID, (avaible_levels_string, difficultyFolderTitle, tableID) =>
                    {
                        string[] avaible_levels_arr = RemoveEmptyLines(avaible_levels_string.Split('\n'));
                        int loadedLevelsCount = 0;

                        for (int i = 0; i < avaible_levels_arr.Length; i++)
                        {
                            string levelFromServerTitle = avaible_levels_arr[i];

                            StartCoroutine(LoadLevelFileContent(currentChosedLanguageKey, difficultyFolderTitle, levelFromServerTitle, (levelContent, difficultyKey) =>
                            {
                                loadedLevelsCount++;
                                LevelScriptableObj levelScriptableObj = new LevelScriptableObj();

                                levelScriptableObj.LevelJson = new TextAsset(levelContent);
                                levelScriptableObj.Parse();

                                _difficultiesLevelsDictionary[difficultyKey].Add(levelScriptableObj);

                                if(loadedLevelsCount == avaible_levels_arr.Length)
                                {
                                    loadedDifficultiesCount++;

                                    LoadedLevelsDictionary._difficultyLevels.Add(
                                        new LanguageWordsDictionary.Difficulty{title = difficultyKey, leaderBoardsTableID = tableID,
                                        _levels = _difficultiesLevelsDictionary[difficultyKey]}
                                        );
                                    

                                    if (loadedDifficultiesCount == avaible_difficulties_arr.Length)
                                    {
                                        //Если это последний уровень в последнем уровне сложности
                                        LoadedLevelsDictionary._difficultyLevels.Sort((x, y) => x.title.CompareTo(y.title));

                                        foreach (var difficulty in LoadedLevelsDictionary._difficultyLevels)
                                        {
                                            difficulty._levels.Sort(((x, y) => x.Title.CompareTo(y.Title)));
                                        }
                                        
                                        NotifyOnLevelsDictionaryLoadListeners();
                                    }

                                }
                            }));
                        }
                    }));
                }
            }));
        }));


    }

    #endregion

    #region Public Methods

    public void RegisterOnLevelsDictionaryLoadListener(System.Action listener)
    {
        if (!_levelsDictionaryLoadedListeners.Contains(listener))
        {
            _levelsDictionaryLoadedListeners.Add(listener);
        }
    }

    public void UnRegisterOnLevelsDictionaryLoadListener(System.Action listener)
    {
        if (_levelsDictionaryLoadedListeners.Contains(listener))
        {
            _levelsDictionaryLoadedListeners.Remove(listener);
        }
    }

    #endregion

    #region Private Methods

    private void NotifyOnLevelsDictionaryLoadListeners()
    {
        foreach (var levelsDictionaryLoadedListener in _levelsDictionaryLoadedListeners)
        {
            levelsDictionaryLoadedListener?.Invoke();
        }
    }

    private string[] RemoveEmptyLines(string[] lines)
    {
        List<string> arr = new List<string>();

        foreach (var item in lines)
        {
            if(item != string.Empty)
            {
                arr.Add(item);
            }
        }

        return arr.ToArray();
    }

    private IEnumerator LoadAvaibleLanguages(System.Action<string> callback)
    {
        string url = BASE_URL + LANGUAGES_AVAIBLE_FILE_TITLE;

        StartCoroutine(GetServerFileContent(url, (avaible_languages_string) =>
        {
            callback?.Invoke(avaible_languages_string);
        }));

        yield return null;
    }

    private IEnumerator LoadAvaibleDifficulties(string currentChosedLanguageKey, System.Action<string> callback)
    {
        string url = BASE_URL + currentChosedLanguageKey + SLASH_STRING + DIFFICULTIES_AVAIBLE_FILE_TITLE;

        StartCoroutine(GetServerFileContent(url, (avaible_difficulties_string) =>
        {
            callback?.Invoke(avaible_difficulties_string);
        }));

        yield return null;
    }

    private IEnumerator LoadAvaibleLevels(string currentChosedLanguageKey, string difficultyFolderTitle, string googleTableID, System.Action<string, string, string> callback)
    {
        string url = BASE_URL + currentChosedLanguageKey + SLASH_STRING + difficultyFolderTitle + SLASH_STRING + LEVELS_AVAIBLE_FILE_TITLE;

        StartCoroutine(GetServerFileContent(url, (avaible_levels_string) =>
        {
            callback?.Invoke(avaible_levels_string, difficultyFolderTitle, googleTableID);
        }));

        yield return null;
    }

    private IEnumerator LoadLevelFileContent(string currentChosedLanguageKey, string difficultyFolderTitle, string levelTitle, System.Action<string, string> callback)
    {
        string url = BASE_URL + currentChosedLanguageKey + SLASH_STRING + difficultyFolderTitle + SLASH_STRING + LEVELS_FOLDER_TITLE + SLASH_STRING + levelTitle;

        StartCoroutine(GetServerFileContent(url, (level_string) =>
        {
            callback?.Invoke(level_string, difficultyFolderTitle);
        }));

        yield return null;
    }

    private IEnumerator GetServerFileContent(string url, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Ошибка при загрузке " + url + " : " + www.error);
            }
            else
            {
                Debug.Log("Завершил считывание файла по пути: " + url);

                if(callback != null)
                {
                    callback(www.downloadHandler.text);
                }
            }
        }
    }

    #endregion

    #endregion
}