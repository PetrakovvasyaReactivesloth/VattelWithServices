using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    #region Methods

    #region Unity Methods

    public void Start()
    {
        // Select the Google Play Games platform as our social platform implementation
        GooglePlayGames.PlayGamesPlatform.Activate();

        // Create client configuration
        PlayGamesClientConfiguration config = new 
            PlayGamesClientConfiguration.Builder()
            .Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;
        
        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    #endregion

    #region Public Methods

    public bool CheckAuth()
    {
        return Social.localUser.authenticated;
    }

    public void SignIn() 
    {
        Social.localUser.Authenticate(SignInCallback);
    }

    #endregion

    #region Callbacks

    public void SignInCallback(bool success) 
    {
        if (success) 
        {
            Debug.Log("Signed in!");
        } 
        
        else 
        {
            Debug.Log("Sign-in failed...");
        }
    }

    public void SignOut()
    {
        ((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut();
    }

    #endregion

    #endregion
}