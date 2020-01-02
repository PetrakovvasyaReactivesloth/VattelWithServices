using UnityEngine;

public class GameBuilder : MonoBehaviour
{
    #region Properties

    public LanguageWordsDictionary LanguageWordsDictionary
    {
        get { return _languageWordsDictionary; }
        set { _languageWordsDictionary = value; }
    }

    #endregion

    #region Private Fields

     private LanguageWordsDictionary _languageWordsDictionary;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        WWWLevelsLoader.Instance.RegisterOnLevelsDictionaryLoadListener(OnLevelsDictionaryLoaded);
    }

    private void OnLevelsDictionaryLoaded()
    {
        _languageWordsDictionary = WWWLevelsLoader.Instance.LoadedLevelsDictionary;
    }

    #endregion

    #endregion
}