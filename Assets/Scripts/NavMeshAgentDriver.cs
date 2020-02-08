using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentDriver : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    // Throw when not busy
    [HideInInspector] public Action OnStopped;

    private void Awake()
    {
        this.navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();

        if (this.navMeshAgent != null)
            this.navMeshAgent.autoRepath = true;
    }

    private void Update()
    {
        // Check for destination reached
        if (this.DestinationReached() && !this.navMeshAgent.isStopped)
            this.Stop();
    }

    private void UpdateDestination(Vector3 newPosition)
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
        bool destinationReached;
        float positionOffsetTolerance = 0.05f;

        Vector3 currentPosition = this.transform.position;
            
        float xDelta = Mathf.Abs(Mathf.Abs(this.navMeshAgent.destination.x) - Mathf.Abs(currentPosition.x));
        float zDelta = Mathf.Abs(Mathf.Abs(this.navMeshAgent.destination.z) - Mathf.Abs(currentPosition.z));
            
        destinationReached = xDelta < positionOffsetTolerance && zDelta < positionOffsetTolerance;
            
        return destinationReached;
    }

    public void MoveTo(Vector3 position)
    {
        this.UpdateDestination(position);
        this.navMeshAgent.isStopped = false;
    }
    
    public void Stop()
    {
        this.navMeshAgent.isStopped = true;
        this.OnStopped?.Invoke();
    }

    public void SetSpeed(float speed)
    {
        this.navMeshAgent.speed = speed;
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint;
        
        do
        {
            NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();

            int randomIndex = Random.Range(0, navMeshTriangulation.indices.Length - 3);

            randomPoint = Vector3.Lerp(navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex]],
                navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex + 1]], Random.value);

            Vector3.Lerp(randomPoint, navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex + 2]],
                Random.value);
        } while (randomPoint.y > 1);

        /* Use this to debug the destination point with a cube
        GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        primitive.GetComponent<Renderer>().material.color = Color.red;

        if (primitive != null)
          primitive.transform.position = randomPoint;
        */

        return randomPoint;
    }
}
