using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    #region Methods

    [SerializeField] private AudioSource source;
    [SerializeField] private float loopCompensation = 0.1f;
    [SerializeField] private AudioClip[] core;

    #endregion

    #region Private Methods

    private AudioClip _currentClip;

    #endregion

    #region Methods

    #region Unity Methods

    private void Start()
    {
        Invoke("PlayClip", loopCompensation);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Вызывается из Invoke
    /// </summary>
    private void PlayClip()
    {
        _currentClip = core[Random.Range(0, core.Length)];
        Invoke("PlayClip", _currentClip.length - loopCompensation);
        source.PlayOneShot(_currentClip);
    }

    #endregion

    #endregion
}