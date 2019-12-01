using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundlesLoader : MonoBehaviour
{
    #region Enums

    private enum ServerType
    {
        Local,
        Global
    }

    #endregion

    #region Constants

    private const int ASSET_BUNDLES_VERSION = 0;
    private const int DISABLE_CHECKING_CRC_CODE = 0;
    private const string OSX_FOLDER_KEY = "StandaloneOSXUniversal";
    private const string WINDOWS_FOLDER_KEY = "StandaloneWindows";
    private const string IOS_FOLDER_KEY = "iOS";
    private const string ANDROID_FOLDER_KEY = "Android";
    private const string LINUX_FOLDER_KEY = "StandaloneLinux";
    private const string CRC_CODES_FILE_NAME = "CRCCodes.txt";
    private const string VERSIONS_CODES_FILE_NAME = "BundlesVersions.txt";
    private const char FILE_ELEMENTS_IN_LINE_DIVIDER = ',';

    #endregion

    #region Events

    public event System.Action AllBundlesLoaded;
    public event System.Action<float> BundlesLoadedPersentageRefreshed;

    #endregion

    #region Properties

    public static AssetBundlesLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("AssetBundlesLoader").AddComponent<AssetBundlesLoader>();
            }

            return _instance;
        }

        set { _instance = value; }
    }

    private int ReadyBundlesCount
    {
        get { return _readyBundlesCount; }
        set
        {
            _readyBundlesCount = value;
            int maxBundles = _crcCodesDictionary.Count;

            float persent = (float) (_readyBundlesCount / (float) maxBundles);

            BundlesLoadedPersentageRefreshed?.Invoke(persent);

            if (ReadyBundlesCount == maxBundles)
            {
                AllBundlesLoaded?.Invoke();
                _allBundlesLoaded = true;
            }
        }
    }

    public LanguageWordsDictionary LoadedLevelsDictionary
    {
        get { return _loadedLevelsDictionary; }
        set { _loadedLevelsDictionary = value; }
    }

    #endregion

    #region Private Fields

    private static AssetBundlesLoader _instance = null;
    private LanguageWordsDictionary _loadedLevelsDictionary;
    private List<string> _loadedBundlesLinks = new List<string>();
    public bool _allBundlesLoaded = false;

    private int _readyBundlesCount;
    private Dictionary<string, uint> _crcCodesDictionary = new Dictionary<string, uint>();
    private Dictionary<string, uint> _bundlesVersionsDictionary = new Dictionary<string, uint>();
    private ServerType _currentServerType = ServerType.Local;
    private List<AssetBundle> _currentLoadedAssetBundles = new List<AssetBundle>();
    private List<System.Action> _levelsDictionaryLoadedListeners = new List<Action>();

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

    private void OnEnable()
    {
        StartGettingBundles();
    }

    private void StartGettingBundles()
    {
        Debug.Log("StartGettingBundles");
        StartCoroutine(LoadInfoFromAssetBundles());
    }

    #endregion

    #region Static Methods

    public static void CleanAllCache()
    {
        Debug.Log(Caching.ClearCache() ? "Cache cleaned successfully" : "Can't clean cache");
    }

    #endregion

    #region Private Methods

    private string GetServerLink()
    {
        string linkBaseGlobal = "https://reactivesloth.com/vpetrakov/vattel-english/AssetBundles/";
        string linkBaseLocal = "https://reactivesloth.com/vpetrakov/vattel-english/AssetBundles/";

        string linkBase = _currentServerType == ServerType.Global ? linkBaseGlobal : linkBaseLocal;

        return (linkBase);
    }

    private IEnumerator GetBundlesNamesAndCRCCodes(System.Action callback = null)
    {
        bool cachedBundlesChecked = false;
        bool isBundlesCached = false;
        bool canUpdate = false;

        Debug.Log("GetBundlesNamesAndCRCCodes");
        UnityWebRequest www = UnityWebRequest.Get(GetServerLink() + GetPlatformString() + "/" + CRC_CODES_FILE_NAME);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Ошибка в доступе к файлу CRC: " + www.error + " по ссылке: " + GetServerLink() + GetPlatformString() + "/" + CRC_CODES_FILE_NAME);
            Debug.Log("Is network error: " + www.isNetworkError);
            Debug.Log("Is http error: " + www.isHttpError);

            if (www.responseCode == 0)
            {
                if (_currentServerType == ServerType.Local)
                {
                    Debug.Log("Проблемы с доступом по локалке, попробую глобалку");
                    _currentServerType = ServerType.Global;
                    //Debug.Log("Пробую остановить корутины, возможно вылетит ошибка");
                    //StopAllCoroutines();
                    StartGettingBundles();

                    yield break;
                }
                else if (_currentServerType == ServerType.Global)
                {
                    Debug.Log("Не получилось получить доступ ни к локальному серверу ни к глобальному");
                }
            }
        }
        else
        {
            canUpdate = true;
            string[] lines = www.downloadHandler.text.Split('\n');

            foreach (var line in lines)
            {
                string[] elements = line.Split(FILE_ELEMENTS_IN_LINE_DIVIDER);

                if (!_crcCodesDictionary.ContainsKey(elements[0]))
                {
                    _crcCodesDictionary.Add(elements[0], uint.Parse(elements[1]));
                }
            }
        }

        StartCoroutine(CheckIsBundlesLoadedInCache((isCached) =>
        {
            Debug.Log("Is cached: " + isCached);

            isBundlesCached = isCached;
            cachedBundlesChecked = true;
        }));

        while (!cachedBundlesChecked)
        {
            Debug.Log("Жду пока проверяются бандлы в кеше");
            yield return null;
        }

        if (!isBundlesCached)
        {
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Нет бандлов в кеше и нет интернета");
                StopAllCoroutines();
            }
            else
            {
                callback?.Invoke();
            }
        }
        else
        {
            if (!canUpdate)
            {
                Debug.Log("Can't update from server, get bundles from cache");
                GetBundles(offlineMode: true);
            }
            else
            {
                callback?.Invoke();
            }
        }
    }

    private IEnumerator CheckVersionsAtServer(System.Action callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(GetServerLink() + VERSIONS_CODES_FILE_NAME);
        yield return www.SendWebRequest();
        Debug.Log("CheckVersionsAtServer");

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Ошибка в доступе к файлу versions: " + www.error);
        }
        else
        {
            string[] lines = www.downloadHandler.text.Split('\n');

            foreach (var line in lines)
            {
                string[] elements = line.Split(FILE_ELEMENTS_IN_LINE_DIVIDER);

                if (!_bundlesVersionsDictionary.ContainsKey(elements[0]))
                {
                    _bundlesVersionsDictionary.Add(elements[0], uint.Parse(elements[1]));
                    //Сохраняем последнее значение версии бандла
                    PlayerPrefs.SetInt(elements[0].ToString(), (int.Parse(elements[1])));
                    PlayerPrefs.Save();
                }
            }
        }

        callback?.Invoke();

        yield return null;
    }

    private bool CheckExistCachedVersions(string bundleName)
    {
        bool result = true;
        List<Hash128> listOfCachedVersions = new List<Hash128>();
        Caching.GetCachedVersions(bundleName, listOfCachedVersions);

        if (listOfCachedVersions.Count == 0)
        {
            Debug.Log(bundleName + " not loaded in cache");

            result = false;
        }

        Debug.Log(bundleName + "cached " + result);
        return result;
    }

    private IEnumerator CheckIsBundlesLoadedInCache(System.Action<bool> callback)
    {
        bool result = false;

        foreach (var bundle_name in _crcCodesDictionary)
        {
            result = CheckExistCachedVersions(bundle_name.Key);

            if (result == false)
            {
                break;
                Debug.Log(bundle_name + "not cached. BREAK LOOP");
            }
        }

        callback?.Invoke(result);
        yield return null;
    }

    private IEnumerator LoadInfoFromAssetBundles()
    {
        StartCoroutine(GetBundlesNamesAndCRCCodes(() =>
        {
            StartCoroutine(CheckVersionsAtServer(() => { GetBundles(); }));
        }));

        yield return null;
    }

    private void GetBundles(bool offlineMode = false)
    {
        Debug.Log("Get bundles : " + offlineMode);
        StartCoroutine(LoadBundlesContinuosly(offlineMode, _crcCodesDictionary.Count));
    }

    private IEnumerator LoadBundlesContinuosly(bool offlineMode, int bundlesToLoadCount)
    {
        int bundlesNeedToLoadCount = bundlesToLoadCount;
        var url = GetServerLink();

        if (bundlesNeedToLoadCount <= 0)
        {
            yield break;
            Debug.Log("All bundles loaded");
        }

        if (_crcCodesDictionary == null || _crcCodesDictionary.Count == 0)
        {
            yield return null;
        }

        StartCoroutine(LoadAssetBundleByURL(url,
            _crcCodesDictionary.Keys.ElementAt(bundlesNeedToLoadCount - 1), offlineMode, () =>
            {
                bundlesNeedToLoadCount--;
                if (bundlesNeedToLoadCount > 0)
                {
                    StartCoroutine(LoadBundlesContinuosly(offlineMode, bundlesNeedToLoadCount));
                }
                else
                {
                    Debug.Log("All bundles loaded");
                    NotifyOnLevelsDictionaryLoadListeners();
                }
            }));
    }

    private IEnumerator LoadAssetBundleByURL(string url, string assetBundleName, bool offlineMode = false,
        System.Action callback = null)
    {
        var errorExist = false;
        var platformString = "";
        var slashString = "/";

        platformString = GetPlatformString();

        var fullLink = url + platformString + slashString + assetBundleName;

        if (_loadedBundlesLinks.Contains(fullLink))
        {
            Debug.Log("Бандл по ссылке " + fullLink + " уже был ранее скачан.");
            yield return null;
        }
        else
        {
            uint crc = offlineMode ? DISABLE_CHECKING_CRC_CODE : _crcCodesDictionary[assetBundleName];
            uint version = 0;

            if (offlineMode)
            {
                if (PlayerPrefs.HasKey(assetBundleName))
                {
                    version = (uint) PlayerPrefs.GetInt(assetBundleName);
                }
                else
                {
                    version = ASSET_BUNDLES_VERSION;
                    PlayerPrefs.SetInt(assetBundleName, ASSET_BUNDLES_VERSION);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                version = _bundlesVersionsDictionary[assetBundleName];
            }

            Debug.Log("Load: " + fullLink + "\n Version: " + version);

            using (UnityWebRequest assetBundlesWebRequest =
                UnityWebRequestAssetBundle.GetAssetBundle(fullLink, version, crc))
            {
                yield return assetBundlesWebRequest.SendWebRequest();

                try
                {
                    if (assetBundlesWebRequest.isNetworkError || assetBundlesWebRequest.isHttpError)
                    {
                        errorExist = true;
                        Debug.LogError("Не могу загрузить бандл: " + fullLink + assetBundlesWebRequest.error);
                        Debug.LogError("isNetworkError: " + assetBundlesWebRequest.isNetworkError);
                        Debug.LogError("isHttpError: " + assetBundlesWebRequest.isHttpError);
                        Debug.LogError("responseCode: " + assetBundlesWebRequest.responseCode);

                        if (assetBundlesWebRequest.responseCode == 0)
                        {
                            Debug.LogError("Проблемы с интернет соединением");
                        }
                    }
                    else
                    {
                        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(assetBundlesWebRequest);
                        var assetsInBundle = bundle.LoadAllAssets();

                        if (!_loadedBundlesLinks.Contains(fullLink))
                        {
                            _loadedBundlesLinks.Add(fullLink);
                        }
                        
                        if (bundle != null)
                        {
                            ReadyBundlesCount++;
                            foreach (var asset in assetsInBundle)
                            {
                                if (asset is LanguageWordsDictionary)
                                {
                                   _loadedLevelsDictionary = asset as LanguageWordsDictionary;
                                }
                            }
                        }

                        //bundle.Unload(true);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }
            }

            if (callback != null && !errorExist)
            {
                callback();
            }
        }

        yield return null;
    }

    private static string GetPlatformString()
    {
        string platformString = "";

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                platformString = OSX_FOLDER_KEY;
                break;
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                platformString = WINDOWS_FOLDER_KEY;
                break;
            case RuntimePlatform.IPhonePlayer:
                platformString = IOS_FOLDER_KEY;
                break;
            case RuntimePlatform.Android:
                platformString = ANDROID_FOLDER_KEY;
                break;
            case RuntimePlatform.LinuxEditor:
            case RuntimePlatform.LinuxPlayer:
                platformString = LINUX_FOLDER_KEY;
                break;
        }

        return platformString;
    }

    public void NotifyOnLevelsDictionaryLoadListeners()
    {
        foreach (var levelsDictionaryLoadedListener in _levelsDictionaryLoadedListeners)
        {
            levelsDictionaryLoadedListener?.Invoke();
        }
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

    #endregion
}