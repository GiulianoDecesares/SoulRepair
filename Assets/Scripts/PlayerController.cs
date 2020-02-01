using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Manage player main logic
/// </summary>
public class PlayerController : MonoBehaviour
{
	[Header("Attack")]
	[SerializeField] private KeyCode attack;
	[SerializeField] private float damagePerHit;

	[Header("Movement")]
	[SerializeField] private float movementSpeed;
	[SerializeField] private float gravity;
	
	private CharacterController characterController;
	private Vector3 moveDirection;
	private List<IEnemy> spottedEnemies;

	private void Awake()
	{
		this.characterController = this.gameObject.GetComponent<CharacterController>();
		this.moveDirection = Vector3.zero;
		this.spottedEnemies = new List<IEnemy>();
	}

	private void Update()
	{
		if (this.characterController != null)
		{
			if (this.characterController.isGrounded)
			{
				this.moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.00f, Input.GetAxis("Vertical"));
				this.moveDirection *= this.movementSpeed;
			}
			
			// Manage other actions here, like jump or something
			if (Input.GetKeyDown(this.attack))
				this.Attack();
			
			// Apply gravity squared
			this.moveDirection.y -= this.gravity * Time.deltaTime;
			this.characterController.Move(this.moveDirection * Time.deltaTime);
		}
		else
		{
			Debug.LogError("Null character controller");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		IEnemy enemy = other.GetComponent<IEnemy>();
	
		if (enemy != null)
		{
			this.spottedEnemies.Add(enemy);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		IEnemy enemy = other.GetComponent<IEnemy>();
	
		if (enemy != null && this.spottedEnemies.Contains(enemy))
		{	
			this.spottedEnemies.Remove(enemy);
		}
	}

	private void Attack()
	{
		foreach (IEnemy enemy in this.spottedEnemies)
			enemy.TakeDamage(this.damagePerHit);
	}
}
