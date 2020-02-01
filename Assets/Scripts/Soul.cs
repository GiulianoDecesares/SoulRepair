using System;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Soul : MonoBehaviour, IEnemy
{
    [SerializeField] private SoulState initialState;

    [SerializeField] private float defaultSpeed;
    [SerializeField] private float followingSpeed;
    
    public enum SoulState { Broken, Healthy };
    private SoulState currentState;
    
    [SerializeField] private RangeTrigger fieldOfView;
    [SerializeField] private RangeTrigger interactionRange;
    
    private NavMeshAgent movementAgent;
    private bool isMoving;
    
    private Soul target;

    private void Awake()
    {
        this.currentState = this.initialState;
        
        this.UpdateVisuals();
        
        this.movementAgent = this.gameObject.GetComponent<NavMeshAgent>();

        this.isMoving = false;
        this.target = null;
    }

    private void Start()
    {
        if (this.movementAgent != null)
        {
            this.movementAgent.speed = this.defaultSpeed;
        }
        else
        {
            Debug.LogError("Null nav mesh argent component");
        }
    }

    private void OnEnable()
    {
        if (this.fieldOfView != null)
        {
            this.fieldOfView.onDetection += this.OnViewEnter;
        }

        if (this.interactionRange != null)
        {
            this.interactionRange.onDetection += this.OnCollision;
        }
    }

    private void OnDisable()
    {
        if (this.fieldOfView != null)
        {
            this.fieldOfView.onDetection -= this.OnViewEnter;
        }

        if (this.interactionRange != null)
        {
            this.interactionRange.onDetection -= this.OnCollision;
        }
    }

    private void Update()
    {
        if (this.target != null)
        {
            this.movementAgent.destination = this.target.gameObject.transform.position;
        }
        else
        {
            this.MoveToRandomSpot();
        }
    }

    private void OnViewEnter(Collider[] others)
    {
        foreach (Collider otherCollider in others)
        {
            Soul otherSoul = otherCollider.GetComponentInParent<Soul>();
        
            if (otherSoul != null)
            {
                if (this.currentState == SoulState.Healthy)
                {
                    if (otherSoul.currentState == SoulState.Healthy)
                    {
                        // Healthy vs healthy       
                    }
                    else if (otherSoul.currentState == SoulState.Broken)
                    {
                        // Healthy vs broken
                    
                        // Run bitch!!
                    }
                }
                else if (this.currentState == SoulState.Broken)
                {
                    if (otherSoul.currentState == SoulState.Healthy)
                    {
                        // Broken vs healthy
                        this.target = otherSoul;
                        this.movementAgent.speed = this.followingSpeed;
                    }
                    else if (otherSoul.currentState == SoulState.Broken)
                    {
                        // Broken vs broken       
                    }
                }
            }
        }
    }
    
    private void OnCollision(Collider[] others)
    {
        foreach (Collider collisionActor in others)
        {
            Soul otherSoul = collisionActor.gameObject.GetComponentInParent<Soul>();

            if (otherSoul != null && !ReferenceEquals(otherSoul, this))
            {
                if (this.currentState == SoulState.Healthy)
                {
                    if (otherSoul.currentState == SoulState.Healthy)
                    {
                        // Healthy catch healthy
                    }
                    else if (otherSoul.currentState == SoulState.Broken)
                    {
                        // Healthy catch broken
                    }
                }
                else if (this.currentState == SoulState.Broken)
                {
                    if (otherSoul.currentState == SoulState.Healthy)
                    {
                        // Broken catch healthy
                        otherSoul.currentState = SoulState.Broken;
                        otherSoul.UpdateVisuals();
                        this.target = null;
                        this.movementAgent.speed = this.defaultSpeed;
                    }
                    else if (otherSoul.currentState == SoulState.Broken)
                    {
                        // Broken catch broken
                    }
                }
            }
        }
    }

    private void MoveToRandomSpot()
    {
        if (!this.isMoving)
        {
            Vector3 newPosition = new Vector3(Random.Range(-25f, 25f), 0.0f, Random.Range(-25f, 25f));

            if (this.movementAgent != null)
            {
                this.movementAgent.SetDestination(newPosition);
                this.isMoving = true;
            }   
        }

        if (!this.movementAgent.pathPending && this.movementAgent.remainingDistance <= 0)
        {
            this.isMoving = false;
        }
    }

    private void ChangeColor(Color color)
    {
        Renderer renderer = this.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogError("Null renderer component");
        }
    }

    public void UpdateVisuals()
    {
        switch (this.currentState)
        {
            case SoulState.Broken:
                this.ChangeColor(Color.red);
                break;
            
            case SoulState.Healthy:
                this.ChangeColor(Color.blue);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log($"Taking damage {damage}");
    }
}
