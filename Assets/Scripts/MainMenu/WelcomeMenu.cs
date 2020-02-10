using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject mainMenuButtonsContent;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject leaderboardsPanel;

    [SerializeField] private Text identificationNameText;
    [SerializeField] private Text leaderboardsPanelText;

    private string userIdentificationName;
    

    public void OnNewGameClick()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            this.mainMenuPanel.SetActive(false);
            this.registerPanel.SetActive(true);   
        }
    }

    public void OnCreditsClick()
    {
        this.mainMenuButtonsContent.SetActive(false);
        this.creditsPanel.SetActive(true);
    }

    public void OnCreditsBackClick()
    {
        this.creditsPanel.SetActive(false);
        this.mainMenuButtonsContent.SetActive(true);
    }

    public void OnRegistrationPanelBeginClick()
    {
        // Disable all buttons
        Button[] registerbuttons = this.registerPanel.GetComponentsInChildren<Button>();
        foreach (Button registerbutton in registerbuttons)
            registerbutton.interactable = false;
        
        // Register the player with id
        LoginWithCustomIDRequest loginRequest = new LoginWithCustomIDRequest
        {
            CustomId = this.identificationNameText.text,
            CreateAccount = true
        };
        
        PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnLoginFail);

        void OnLoginSuccess(LoginResult result)
        {
            // Catch the session ticket
            PlayFabSettings.staticPlayer.ClientSessionTicket = result.SessionTicket;
            
            this.registerPanel.SetActive(false);
            this.mainMenuPanel.SetActive(true);
        }

        void OnLoginFail(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
            
            foreach (Button registerbutton in registerbuttons)
                registerbutton.interactable = true;
        }
    }

    public void OnRegistrationPanelBackClick()
    {
        this.registerPanel.SetActive(false);
        this.mainMenuPanel.SetActive(true);
    }

    public void OnIdentificationNameFieldChanged()
    {
        this.userIdentificationName = this.identificationNameText.text;
    }

    public void OnLeaderboardsClick()
    {
        this.mainMenuPanel.SetActive(false);
        this.leaderboardsPanel.SetActive(true);
        
        // Check if user is logged
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            return;
        }
        
        // Get leaderboards request
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            MaxResultsCount = 10,
            StatisticName = "HighScore"
        };
        
        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest, OnGetLeaderboardSuccess, OnGetLeaderboardFailed);

        void OnGetLeaderboardSuccess(GetLeaderboardResult leaderboardResult)
        {
            this.leaderboardsPanelText.text = "";
            
            foreach (PlayerLeaderboardEntry entry in leaderboardResult.Leaderboard)
            {
                this.leaderboardsPanelText.text += $"{entry.DisplayName} : {entry.StatValue} \n";
            }
        }

        void OnGetLeaderboardFailed(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
        }
    }

    public void OnLeaderboardBackClick()
    {
        this.leaderboardsPanel.SetActive(false);
        this.mainMenuPanel.SetActive(true);
    }

    public void OnExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
