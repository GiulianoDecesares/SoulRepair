﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SoulManager : MonoBehaviour
{
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private UserInterface userInterface;

    [Header("Configuration")]
    [SerializeField] private uint brokenSoulsAmount;
    [SerializeField] private uint healthySoulsAmount;

    public int CurrentBrokenSoulsAmount { private set; get; }
    public int CurrentNormalSoulsAmount { private set; get; }

    // TODO :: Possible design fail here
    private bool gameHasStarted;

    private void Awake()
    {
        this.gameHasStarted = false;
        
        this.CurrentBrokenSoulsAmount = 0;
        this.CurrentNormalSoulsAmount = 0;
    }

    private void Start()
    {
        // Instantiate all souls
        for (uint soulIndex = 0; soulIndex < this.brokenSoulsAmount + this.healthySoulsAmount; soulIndex++)
        {
            Soul soul = this.InstantiateSoul(this.GetRandomNavMeshPosition());

            if (soulIndex < this.brokenSoulsAmount)
            {
                soul.Broke();
                this.CurrentBrokenSoulsAmount++;
            }
            else
            {
                soul.Repair();
                this.CurrentNormalSoulsAmount++;
            }
        }

        this.gameHasStarted = true;
        this.userInterface.OnSoulRatioChanged(this);
    }

    private Soul InstantiateSoul(Vector3 position)
    {
        GameObject soulGameObject = Instantiate(this.soulPrefab, position, Quaternion.identity);
        Soul soul = soulGameObject.GetComponent<Soul>();

        if (soul != null)
        {
            soul.Initialize(this);
        }
        else
        {
            Debug.LogError("Null soul component");
        }

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

    public void OnSoulBroke()
    {
        if (this.gameHasStarted)
        {
            this.CurrentNormalSoulsAmount--;
            this.CurrentBrokenSoulsAmount++;

            // Notify UI
            this.userInterface.OnSoulRatioChanged(this);
            
            this.CheckForWinCondition();
        }
    }

    public void OnSoulRepaired()
    {
        if (this.gameHasStarted)
        {
            this.CurrentBrokenSoulsAmount--;
         
            // Notify UI
            this.userInterface.OnSoulRatioChanged(this);
            
            this.CheckForWinCondition();
        }
    }

    private void CheckForWinCondition()
    {
        if (this.CurrentBrokenSoulsAmount <= 0)
        {
            // Win
            this.userInterface.OnGameFinished(true);
            StartCoroutine(this.MainMenuCoroutine());
        } 
        else if (this.CurrentNormalSoulsAmount <= 0)
        {
            // Lose
            this.userInterface.OnGameFinished(false);
            StartCoroutine(this.MainMenuCoroutine());
        }
    }

    private IEnumerator MainMenuCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
