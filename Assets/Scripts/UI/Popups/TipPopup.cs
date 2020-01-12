using UnityEngine;
using UnityEngine.UI;

public class TipPopup : Popup
{
    #region Serialized Fields

    [SerializeField] private Button _closePopupButton;
    [SerializeField] private Text _mainTipText;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        _closePopupButton.onClick.AddListener(() => { Hide(); });
    }

    #endregion

    #region Public Methods

    public void Init(LevelScriptableObj.Word word)
    {
        _mainTipText.text = word.word.ToUpper() + "\n" + word.tip.ToUpper();
    }

    #endregion

    #endregion
}