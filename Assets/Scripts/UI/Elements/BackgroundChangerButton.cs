using UnityEngine;
using UnityEngine.UI;

public class BackgroundChangerButton : MonoBehaviour
{
    #region Private Fields

    private Image _cachedImage;
    private Button _cachedButton;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _cachedImage = GetComponent<Image>();
        _cachedButton = GetComponent<Button>();

        _cachedButton.onClick.AddListener(() =>
        {
            BackgroundManager.Instance.SetBackgroundImageSprite(_cachedImage.sprite);
        });
    }

    #endregion

    #region Public Methods

    public void Init(Sprite sprite)
    {
        _cachedImage.sprite = sprite;
    }

    #endregion

    #endregion
}