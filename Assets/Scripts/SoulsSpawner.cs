using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SoulsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject soulPrefab;

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
        for (uint soulIndex = 0; soulIndex < this.brokenSoulsAmount; soulIndex++)
        {
            Vector3 randomPosition = Vector3.zero;

            do
            {
                randomPosition = this.GetRandomNavMeshPosition();
            } while (randomPosition.y > 1);

            GameObject soulGameObject = Instantiate(this.soulPrefab, randomPosition, Quaternion.identity);
            Soul soul = soulGameObject.GetComponent<Soul>();
            soul?.SetState(Soul.SoulState.Broken);
            this.SubscribeToSoul(soul);
        }
        
        for (uint soulIndex = 0; soulIndex < this.healthySoulsAmount; soulIndex++)
        {
            Vector3 randomPosition = Vector3.zero;

            do
            {
                randomPosition = this.GetRandomNavMeshPosition();
            } while (randomPosition.y > 1);
            
            GameObject soulGameObject = Instantiate(this.soulPrefab, randomPosition, Quaternion.identity);
            Soul soul = soulGameObject.GetComponent<Soul>();
            soul?.SetState(Soul.SoulState.Healthy);
            this.SubscribeToSoul(soul);
        }
    }

    private void SubscribeToSoul(Soul soul)
    {
        if (soul != null)
        {
            soul.onDieActionQueue.Enqueue(this.OnSoulDie);
        }
    }

    private void OnSoulDie(Soul soul)
    {
        if (this.instantiatedSouls.Contains(soul))
            this.instantiatedSouls.Remove(soul);
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
}
