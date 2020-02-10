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

    [SerializeField] private Text identificationNameText;

    private string userIdentificationName;
    
    public void OnNewGameClick()
    {
        this.mainMenuPanel.SetActive(false);
        this.registerPanel.SetActive(true);
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

            // Load playable scene
            SceneManager.LoadScene(1);
        }

        void OnLoginFail(PlayFabError error)
        {
            Debug.Log(error.GenerateErrorReport());
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

    public void OnExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
