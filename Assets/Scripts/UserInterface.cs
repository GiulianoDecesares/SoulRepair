using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private float currentHealthySoulsAmount = 0;
    private float currentBrokenSoulsAmount = 0;
    
    public void OnSoulCreated(Soul soul)
    {
        if (soul.GetState() == Soul.SoulState.Broken)
        {
            this.currentBrokenSoulsAmount++;
        }
        else if (soul.GetState() == Soul.SoulState.Healthy)
        {
            this.currentHealthySoulsAmount++;
        }
        
        this.UpdateSlider();
    }

    public void OnSoulDestroyed(Soul soul)
    {
        if (soul.GetState() == Soul.SoulState.Broken)
        {
            if (this.currentBrokenSoulsAmount > 0)
                this.currentBrokenSoulsAmount--;
        }
        else if (soul.GetState() == Soul.SoulState.Healthy)
        {
            if (this.currentHealthySoulsAmount > 0)
                this.currentHealthySoulsAmount--;
        }
        
        this.UpdateSlider();
    }

    public void OnSoulChanged(Soul soul, Soul.SoulState previousState)
    {
        if (previousState == Soul.SoulState.Broken)
        {
            if (this.currentBrokenSoulsAmount > 0)
                this.currentBrokenSoulsAmount--;

            this.currentHealthySoulsAmount++;
        }
        else if (previousState == Soul.SoulState.Healthy)
        {
            if (this.currentHealthySoulsAmount > 0)
                this.currentHealthySoulsAmount--;

            this.currentBrokenSoulsAmount++;
        }
        
        this.UpdateSlider();
    }

    private void UpdateSlider()
    {
        if (this.slider != null)
        {
            this.slider.value = currentBrokenSoulsAmount / (float)(this.currentBrokenSoulsAmount + this.currentHealthySoulsAmount);
        }
        else
        {
            Debug.LogError("Null slider reference");
        }
    }
}
