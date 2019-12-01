using UnityEngine;

public class Menu : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject menuScreen, gamePanel, exitScreen;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _uiClicksSoundArray, _screenTransitionsSoundArray;

    #endregion

    #region Methods

    #region Public Methods

    public void OpenMenuScreen()
    {
        PlaySound(true);

        menuScreen.SetActive(true);
        exitScreen.SetActive(false);
        gamePanel.SetActive(false);
    }

    /// <summary>
    /// Вызывается из Inspector на кнопке Play
    /// </summary>
    public void OpenLevelScreen()
    {
        PlaySound(true);

        menuScreen.SetActive(false);
        exitScreen.SetActive(false);
        gamePanel.SetActive(true);
    }

    /// <summary>
    /// Вызывается из Inspector на кнопке Quit
    /// </summary>
    public void OpenExit()
    {
        PlaySound(true);

        menuScreen.SetActive(false);
        exitScreen.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void LoadLevel(int i)
    {
        PlaySound(true);

        menuScreen.SetActive(false);
        exitScreen.SetActive(false);
        gamePanel.SetActive(true);
    }

    /// <summary>
    /// Вызывается из Inspector на кнопке ApplyButton в QuitMenu
    /// </summary>
    public void Exit()
    {
        PlaySound(false);
        Application.Quit();
    }

    public void PlaySound(bool playTransition)
    {
        if (playTransition)
            Invoke("PlayTransition", 0.1f);
        else
            _audioSource.PlayOneShot(_uiClicksSoundArray[Random.Range(0, _uiClicksSoundArray.Length)]);
    }

    /// <summary>
    /// Вызывается из Invoke
    /// </summary>
    private void PlayTransition()
    {
        _audioSource.PlayOneShot(_screenTransitionsSoundArray[Random.Range(0, _screenTransitionsSoundArray.Length)]);
    }

    #endregion

    #endregion
}