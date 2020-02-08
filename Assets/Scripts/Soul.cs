using System;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Soul : MonoBehaviour, ISoul
{
    // Inspector references
    [SerializeField] private RangeTrigger fieldOfView;
    [SerializeField] private RangeTrigger interactionRange;
    
    // Private references
    private ISoulBehavior behavior;
    
    private NavMeshAgentDriver movementAgentDriver;
    private SoulManager manager;
    
    private bool isMoving;
    public bool isBroken { private set; get; }

    private void MoveToRandomSpot()
    {
        if (this.movementAgentDriver != null)
        {
            this.movementAgentDriver.MoveTo(this.movementAgentDriver.GetRandomPointOnNavMesh());
        }
        else
        {
            Debug.LogError("Null movement agent driver reference");
        }
    }

    public void Initialize(SoulManager parameterManager)
    {
        this.manager = parameterManager;
        
        this.movementAgentDriver = this.gameObject.GetComponent<NavMeshAgentDriver>();
        
        if (this.movementAgentDriver != null)
        {
            this.movementAgentDriver.OnStopped += this.MoveToRandomSpot;
        }
        else
        {
            Debug.LogError("Null agent driver reference");
        }
    }

    public void SetMovementSpeed(float newMovementSpeed)
    {
        this.movementAgentDriver.SetSpeed(newMovementSpeed);
    }

    public void SetTargetToFollow(Soul parameterTarget)
    {
        // this.movementAgentDriver.Follow(parameterTarget.transform);
    }

    private void UpdateEvents()
    {
        // TODO :: This could definitively fail at any moment
        
        // Wipe out events
        if (this.fieldOfView != null)
            this.fieldOfView.onDetection = null;

        if (this.interactionRange != null)
            this.interactionRange.onDetection = null;


        // Subscribe new functionality
        if (this.fieldOfView != null)
            this.fieldOfView.onDetection += this.behavior.OnFieldOfViewEnter;

        if (this.interactionRange != null)
            this.interactionRange.onDetection += this.behavior.OnCollisionEnter;
    }

    public void Broke()
    {
        this.behavior = new BrokenSoulBehavior(this);
        this.behavior.Initialize();

        this.UpdateEvents();

        this.isBroken = true;
        
        // Notify manager
        this.manager.OnSoulBroke();
    }

    public void Repair()
    {
        this.behavior = new HealthySoulBehavior(this);
        this.behavior.Initialize();

        this.UpdateEvents();

        this.isBroken = false;
        
        // Notify manager
        this.manager.OnSoulRepaired();
    }
}
