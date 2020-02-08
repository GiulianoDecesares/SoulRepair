using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class BrokenSoulBehavior : ISoulBehavior
{
    private Soul soul;

    private float followingSpeed = 10f;
    private float normalSpeed = 5f;

    public BrokenSoulBehavior(Soul parameterSoul)
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
                renderer.material.color = Color.red;
            }
            else
            {
                Debug.LogError("Null soul renderer");
            }
        }
    }

    public void OnFieldOfViewEnter(Collider[] others)
    {
        Soul targetSoul = null;
        
        foreach (Collider collider in others)
        {
            Soul otherSoul = collider.GetComponent<Soul>();

            if (otherSoul != null)
            {
                // Take the first one
                targetSoul = otherSoul;
                break;
            }
        }

        if (targetSoul != null)
        {
            if (targetSoul.IsBroken)
            {
                // Broken vs broken
            }
            else 
            {
                // Broken vs healthy
                this.soul.SetTargetToFollow(targetSoul);
                this.soul.SetMovementSpeed(this.followingSpeed);
            }
        }
    }

    public void OnCollisionEnter(Collider[] others)
    {
        foreach (Collider otherCollider in others)
        {
            Soul otherSoul = otherCollider.GetComponentInParent<Soul>();
            
            if (otherSoul != null)
            {
                // Broken soul catch healthy soul
                // Broke every soul we touch <3
                otherSoul.Broke();
                this.soul.SetTargetToFollow(null);
            }
        }
    }
}
