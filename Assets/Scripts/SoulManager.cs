using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SoulManager : MonoBehaviour
{
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private UserInterface userInterface;

    [Header("Configuration")]
    [SerializeField] private uint brokenSoulsAmount;
    [SerializeField] private uint healthySoulsAmount;

    private List<Soul> instantiatedSouls;

    private void Awake()
    {
        this.instantiatedSouls = new List<Soul>();
    }

    private void Start()
    {
        // Instantiate all souls
        for (uint soulIndex = 0; soulIndex < this.brokenSoulsAmount + this.healthySoulsAmount; soulIndex++)
        {
            Soul soul = this.InstantiateSoul(this.GetRandomNavMeshPosition(), soulIndex < this.brokenSoulsAmount ? Soul.SoulState.Broken : Soul.SoulState.Healthy);
            this.userInterface.OnSoulCreated(soul); 
        }
    }

    private Soul InstantiateSoul(Vector3 position, Soul.SoulState initialState)
    {
        GameObject soulGameObject = Instantiate(this.soulPrefab, position, Quaternion.identity);
        Soul soul = soulGameObject.GetComponent<Soul>();
        soul?.Initialize(this, initialState);

        return soul;
    }
    
    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPoint = Vector3.zero;
        
        do
        {
            NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();

            int randomIndex = Random.Range(0, navMeshTriangulation.indices.Length - 3);

            randomPoint = Vector3.Lerp(navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex]],
                navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex + 1]], Random.value);

            Vector3.Lerp(randomPoint, navMeshTriangulation.vertices[navMeshTriangulation.indices[randomIndex + 2]],
                Random.value);
        } while (randomPoint.y > 1);

        return randomPoint;
    }
    
    public void OnSoulDie(Soul soul)
    {
        if (this.instantiatedSouls.Contains(soul))
            this.instantiatedSouls.Remove(soul);
        
        this.userInterface.OnSoulDestroyed(soul);

        this.CheckForWinCondition();
    }

    public void OnSoulChanged(Soul soul, Soul.SoulState previousState)
    {
        this.userInterface.OnSoulChanged(soul, previousState);
    }

    private void CheckForWinCondition()
    {
        if (this.instantiatedSouls.Where(soul => soul.GetState() == Soul.SoulState.Broken).ToList().Count == 0)
        {
            // Win
        }
        else if (this.instantiatedSouls.Where(soul => soul.GetState() == Soul.SoulState.Healthy).ToList().Count == 0)
        {
            // Lose
            Debug.Log("Loose");
        }
    }
}
