using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheDeathAnimator : MonoBehaviour
{
    private Animator animator;
    
    private readonly int walkName = Animator.StringToHash("Walk");
    private readonly int stopName = Animator.StringToHash("Stop");
    private readonly int slashName = Animator.StringToHash("Slash");

    private void Awake()
    {
        this.animator = this.gameObject.GetComponent<Animator>();
    }

    public void Walk(float walkSpeed)
    {
        if (this.animator != null)
        {
            this.animator.SetFloat(this.walkName, walkSpeed);
        }
    }

    public void Stop()
    {
        if (this.animator != null)
        {
            this.animator.SetTrigger(this.stopName);
        }
    }

    public void Slash()
    {
        if (this.animator != null)
        {
            this.animator.SetTrigger(this.slashName);
        }
    }
}
