using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IEnemy
{
    public void TakeDamage(float damage)
    {
        Debug.Log($"Taking damage {damage}");
    }
}
