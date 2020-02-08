using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class HealthySoulBehavior : ISoulBehavior
{
    private Soul soul;
    
    private float normalSpeed = 5f;

    public HealthySoulBehavior(Soul parameterSoul)
    {
        this.soul = parameterSoul;
    }
    
    public void Initialize()
    {
        this.soul.SetMovementSpeed(this.normalSpeed);
        this.UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (this.soul != null)
        {
            Renderer renderer = this.soul.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = Color.blue;
            }
            else
            {
                Debug.LogError("Null soul renderer");
            }
        }
    }

    public void OnFieldOfViewEnter(Collider[] others)
    {
        
    }

    public void OnCollisionEnter(Collider[] others)
    {
        
    }
}
