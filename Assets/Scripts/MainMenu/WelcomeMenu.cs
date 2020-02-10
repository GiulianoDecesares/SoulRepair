using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject mainMenuButtonsContent;
    [SerializeField] private GameObject creditsPanel;
    
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
        SceneManager.LoadScene(1);
    }

    public void OnRegistrationPanelBackClick()
    {
        this.registerPanel.SetActive(false);
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
