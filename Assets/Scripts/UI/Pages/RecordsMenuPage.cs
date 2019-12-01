using UnityEngine;
using UnityEngine.UI;

public class RecordsMenuPage : Page
{
    #region Serialized Fields

    [SerializeField] private Button _backToMainMenuButton;
    [SerializeField] private MainMenuPage _mainMenuPage;

    #endregion

    #region Mehtods

    #region Unity Methods

    private void Awake()
    {
        _backToMainMenuButton.onClick.AddListener(() =>
        {
            Hide();
            _mainMenuPage.Show();
        });
    }

    #endregion

    #endregion
}