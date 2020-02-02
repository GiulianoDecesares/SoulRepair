using System;
using System.Collections.Generic;
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

    [SerializeField] private Renderer modelRenderer;
    
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float followingSpeed;

    [SerializeField] private float life;
    
    [SerializeField] private RangeTrigger fieldOfView;
    [SerializeField] private RangeTrigger interactionRange;
    
    public enum SoulState { Broken, Healthy };
    private SoulState currentState;
    
    private NavMeshAgent movementAgent;
    private bool isMoving;
    
    private Soul target;

    public Queue<System.Action<Soul>> onDieActionQueue;
    
    private void Awake()
    {
        this.movementAgent = this.gameObject.GetComponent<NavMeshAgent>();
        this.onDieActionQueue = new Queue<Action<Soul>>();

        this.isMoving = false;
        this.target = null;
        
        this.SetState(this.initialState);
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
            if (this.target.currentState == SoulState.Healthy)
                this.movementAgent.destination = this.target.gameObject.transform.position;
            else
                this.target = null;
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
                        otherSoul.SetState(SoulState.Broken);
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
    
    private Vector3 GetRandomNavMeshPosition()
    {
        NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();

        int randomIndex = Random.Range(0, navMeshTriangulation.indices.Length - 3);

        Vector3 randomPoint = Vector3.Lerp(navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex]],
            navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex + 1]], Random.value);

        Vector3.Lerp(randomPoint, navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex + 2]],
            Random.value);

        return randomPoint;
    }

    private void MoveToRandomSpot()
    {
        if (!this.isMoving)
        {
            Vector3 newPosition = this.GetRandomNavMeshPosition(); // new Vector3(Random.Range(-25f, 25f), 0.0f, Random.Range(-25f, 25f));

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
        if (this.modelRenderer != null)
        {
            this.modelRenderer.material.color = color;
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

    public void SetState(SoulState newState)
    {
        this.currentState = newState;
        this.UpdateVisuals();
    }
    
    public void TakeDamage(float damage)
    {
        this.life -= damage;

        if (this.IsDead())
        {
            while (this.onDieActionQueue.Count > 0)
                this.onDieActionQueue.Dequeue()?.Invoke(this);
            
            Destroy(this.gameObject);
        }
    }

    public bool IsDead()
    {
        return this.life <= 0;
    }
}
