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
            if (this.slider != null)
            {
                this.slider.value = sender.CurrentBrokenSoulsAmount / (float)sender.TotalSoulsAmount;
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
}
