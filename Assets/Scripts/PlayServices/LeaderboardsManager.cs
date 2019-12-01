using GooglePlayGames;
using UnityEngine;

public class LeaderboardsManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private AuthManager _authManager;

    #endregion
    
    #region Methods

    #region Public Methods

    public void ShowLeaderboard(string leaderboardID) 
    {
        if (_authManager.CheckAuth()) 
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardID);
        }
        else 
        {
          _authManager.SignIn();
        }
    }

    public void ShowAllLeaderboards() 
    {
        if (_authManager.CheckAuth()) 
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else 
        {
          _authManager.SignIn();
        }
    }

    public void PostScores(int scores, string tableID)
    {
        // Submit leaderboard scores, if authenticated
        if (_authManager.CheckAuth())
        {
            // Note: make sure to add 'using GooglePlayGames'
            PlayGamesPlatform.Instance.ReportScore(scores,
                tableID,
                (bool success) =>
                {
                    Debug.Log("Leaderboard update success: " + success);
                });
        }
        else
        {
            //_authManager.SignIn();
        }
    }

    #endregion

    #endregion
}