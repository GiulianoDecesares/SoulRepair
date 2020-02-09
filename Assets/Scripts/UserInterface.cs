using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Text gameFeedbackText;

    public void OnSoulRatioChanged(SoulManager sender)
    {
        if (sender != null)
        {
            if (this.slider != null)
            {
                this.slider.value = sender.CurrentBrokenSoulsAmount / (float) (sender.CurrentNormalSoulsAmount + sender.CurrentBrokenSoulsAmount);
            }
            else
            {
                Debug.LogError("Null slider reference");
            }
        }
        else
        {
            Debug.LogError("Null sender reference");
        }
    }
    
    public void OnGameFinished(bool win)
    {
        if (this.gameFeedbackText != null)
        {
            this.gameFeedbackText.enabled = true;

            if (win)
            {
                this.gameFeedbackText.text = "You win!";
            }
            else
            {
                this.gameFeedbackText.text = "You lose!";
            }
        }
    }
}
