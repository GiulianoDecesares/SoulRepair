using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject CreditsContent;
    public GameObject MainMenuContent;

    private void Awake()
    {
        EnableCredits(false);
        EnableMain(true);
        
        Callbacks.OnExitCreditsClick += ExitCreditsToMenu;
        Callbacks.OnCreditsClick += OnEnterCredits;
    }

    private void OnDestroy()
    {
        Callbacks.OnExitCreditsClick -= ExitCreditsToMenu;
        Callbacks.OnCreditsClick -= OnEnterCredits;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
        // Callbacks.NewGameClick();
    }

    public void Credits()
    {
        Callbacks.CreditsClick();
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
        
        // Callbacks.ExitClick();
    }

    public void ExitCredits()
    {
        Callbacks.ExitCreditsClick();
    }

    private void ExitCreditsToMenu()
    {
        EnableCredits(false);
        EnableMain(true);
    }

    private void OnEnterCredits()
    {
        EnableCredits(true);
        EnableMain(false);
    }

    private void EnableCredits(bool value)
    {
        CreditsContent.SetActive(value);
    }

    private void EnableMain(bool value)
    {
        MainMenuContent.SetActive(value);
    }

    public static class Callbacks
    {
        public static Action OnNewGameClick;

        public static void NewGameClick()
        {
            OnNewGameClick?.Invoke();
        }

        public static Action OnCreditsClick;

        public static void CreditsClick()
        {
            OnCreditsClick?.Invoke();
        }

        public static Action OnExitClick;

        public static void ExitClick()
        {
            OnExitClick?.Invoke();
        }

        public static Action OnExitCreditsClick;

        public static void ExitCreditsClick()
        {
            OnExitCreditsClick?.Invoke();
        }
    }
}