using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentDriver : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    
    private Transform targetTransform;
    private Vector3? targetPosition;

    private bool hasNotified;

    // Throw when not busy
    public Action OnIdle;

    private void Awake()
    {
        this.navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for target to follow
        if (this.targetTransform != null)
            this.targetPosition = this.targetTransform.position;
        
        // Check for current target position to go
        if (this.targetPosition.HasValue)
        {
            this.hasNotified = false;
            this.SetNavMeshPosition(this.targetPosition.Value);
        }
        
        // Check for destination reached
        if (this.DestinationReached())
        {
            this.Stop();
        }
        
        // Check for notification state 
        if (this.targetPosition.HasValue == false && this.hasNotified == false)
        {
            this.OnIdle?.Invoke();
            this.hasNotified = true;
        }
    }

    private void SetNavMeshPosition(Vector3 newPosition)
    {
        if (this.navMeshAgent != null)
        {
            this.navMeshAgent.destination = newPosition;
        }
        else
        {
            Debug.LogError("Null nav mesh agent reference");
        }
    }

    private bool DestinationReached()
    {
        // Maybe some tolerance is needed here
        return this.targetPosition == this.transform.position;
    }

    public void MoveTo(Vector3 position)
    {
        this.Stop();
        this.SetNavMeshPosition(position);
    }

    public void Follow(Transform transformReference)
    {
        this.targetTransform = transformReference;
    }

    public void Stop()
    {
        this.targetTransform = null;
        this.targetPosition = null;
    }

    public void SetSpeed(float speed)
    {
        this.navMeshAgent.speed = speed;
    }
}
