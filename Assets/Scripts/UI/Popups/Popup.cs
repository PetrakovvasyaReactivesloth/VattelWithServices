using UnityEngine;

public class Popup : MonoBehaviour
{
    #region Methods

    #region Virtual Methods

    public virtual void Hide()
    {
        ITweenHelper(
            gameObject, "DisablePopup",
            Vector3.zero,
            iTween.EaseType.linear
        );
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);

        const float time = 0.2f;
        ITweenHelper(gameObject, "", Vector3.one, iTween.EaseType.linear, time, "ResetScale");
    }

    #endregion

    #region Private Methods

    private void ITweenHelper(GameObject obj, string onComplete, Vector3 scale, iTween.EaseType easyType,
        float time = 0.1f, string onStart = "")
    {
        iTween.ScaleTo(
            obj,
            iTween.Hash(
                "scale", scale,
                "time", time,
                "easetype", easyType,
                "oncomplete", onComplete,
                "onstart", onStart
            )
        );
    }

    /// <summary>
    /// Вызывается из oncomplete iTween
    /// </summary>
    private void ResetScale()
    {
        transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// Вызывается из oncomplete iTween
    /// </summary>
    private void DisablePopup()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #endregion
}