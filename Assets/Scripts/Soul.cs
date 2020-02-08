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
    
    private Soul targetToFollow;
    
    public bool IsBroken { private set; get; }

    private void Update()
    {
        if (this.targetToFollow != null)
            this.movementAgentDriver.MoveTo(this.targetToFollow.transform.position);
    }

    public void Initialize(SoulManager parameterManager)
    {
        this.manager = parameterManager;
        
        this.movementAgentDriver = this.gameObject.GetComponent<NavMeshAgentDriver>();
        
        if (this.movementAgentDriver != null)
        {
            this.movementAgentDriver.OnStopped += () =>
            {
                if (this.movementAgentDriver != null)
                {
                    this.movementAgentDriver.MoveTo(this.movementAgentDriver.GetRandomPointOnNavMesh());
                }
                else
                {
                    Debug.LogError("Null movement agent driver reference");
                }
            };
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
        this.targetToFollow = parameterTarget;
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
        // This check sucks
        if (this.behavior is HealthySoulBehavior || this.behavior == null)
        {
            this.behavior = new BrokenSoulBehavior(this);
            this.behavior.Initialize();

            this.UpdateEvents();

            this.IsBroken = true;
        
            // Notify manager
            this.manager.OnSoulBroke();
        }
        
    }

    public void Repair()
    {
        // This check sucks
        if (this.behavior is BrokenSoulBehavior || this.behavior == null)
        {
            /*
            this.behavior = new HealthySoulBehavior(this);
            this.behavior.Initialize();

            this.UpdateEvents();

            this.IsBroken = false;
            */

            // Notify manager
            this.manager.OnSoulRepaired();
            
            Destroy(this.gameObject);
        }
    }
}
