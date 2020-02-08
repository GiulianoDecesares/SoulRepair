using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void OnSoulRatioChanged(SoulManager sender)
    {
        if (sender != null)
        {
            this.UpdateSlider(sender.currentBrokenSoulsAmount, sender.currentHealthySoulsAmount);   
        }
        else
        {
            Debug.LogError("Null sender reference");
        }
    }

    private void UpdateSlider(int brokenSoulsAmount, int healthySoulsAmount)
    {
        if (this.slider != null)
        {
            this.slider.value = brokenSoulsAmount / (float)(healthySoulsAmount + brokenSoulsAmount);
        }
        else
        {
            Debug.LogError("Null slider reference");
        }
    }
}
