using UnityEngine;

public class FPSOptimizer : MonoBehaviour
{
    #region Constants

    private const int FPS = 60;

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        Application.targetFrameRate = FPS;
    }

    #endregion

    #endregion
}