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
        this.CheckForWinCondition();
    }

    private void CheckForWinCondition()
    {
        

        /*
        if (!brokenInstantiatedSouls.Any())
        {
            // Win
        }
        else if (!healthyInstantiatedSouls.Any())
        {
            // Lose
            Debug.Log("Loose");
        }

        Debug.Log("Checking for win condition");

        foreach (Soul soul in brokenInstantiatedSouls)
        {
            Debug.Log("Broken -> " + soul);
        }
        
        foreach (Soul soul in healthyInstantiatedSouls)
        {
            Debug.Log("Healthy -> " + soul);
        }*/
    }
}
