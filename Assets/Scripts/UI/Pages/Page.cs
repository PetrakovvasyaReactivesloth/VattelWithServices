using UnityEngine;

public class Page : MonoBehaviour
{
    #region Methods

    #region Virtual Methods

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #endregion
}