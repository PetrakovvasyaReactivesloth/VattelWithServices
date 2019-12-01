using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    #region Properties

    public static BackgroundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<BackgroundManager>();
                _instance.name = "BackgroundManager";
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public Sprite[] BackgroundSprites
    {
        get { return _backgroundSprites; }
        set { _backgroundSprites = value; }
    }

    #endregion
    
    #region Static Fields

    public static BackgroundManager _instance = null;

    #endregion

    #region Serialized Fields

    [SerializeField] private Sprite[] _backgroundSprites;

    #endregion

    #region Private Fields

    private Image _backgroundImage;

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

        _backgroundImage = GetComponent<Image>();
    }

    private void Start()
    {
        string savedSpriteName = PlayerPrefsManager.GetSavedBackgroundSpriteName();
        Sprite temp = BackgroundSprites.FirstOrDefault(spr => spr.name == savedSpriteName);
        SetBackgroundImageSprite(temp);
    }

    #endregion

    #region Public Methods

    public void SetBackgroundImageSprite(Sprite sprite)
    {
        if (sprite != null)
        {
            _backgroundImage.sprite = sprite;
            PlayerPrefsManager.SaveChoosedBackgroundSprite(sprite);
        }
        else
        {
            Debug.LogError("Не был найден сохраненный ранее спрайт фона в массиве фонов в BackgroundManager или был передан пустой спрайт");
        }
    }

    #endregion

    #endregion
}