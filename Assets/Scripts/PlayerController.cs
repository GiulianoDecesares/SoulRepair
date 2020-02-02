using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	
	private List<WeakReference<IEnemy>> spottedEnemies;

	private void Awake()
	{
		this.characterController = this.gameObject.GetComponent<CharacterController>();
		this.moveDirection = Vector3.zero;
		this.spottedEnemies = new List<WeakReference<IEnemy>>();
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
			this.spottedEnemies.Add(new WeakReference<IEnemy>(enemy));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		IEnemy otherEnemy = other.GetComponent<IEnemy>();

		if (otherEnemy != null)
		{
			List<WeakReference<IEnemy>> selectedEnemies = this.spottedEnemies.Where(item =>
			{
				bool result = false;

				if (item.TryGetTarget(out IEnemy localEnemyReference))
				{
					result = ReferenceEquals(otherEnemy, localEnemyReference);
				}

				return result;
			}).ToList();

			foreach (WeakReference<IEnemy> selectedEnemy in selectedEnemies)
			{
				this.spottedEnemies.Remove(selectedEnemy);
			}
		}
	}

	private void Attack()
	{
		foreach (WeakReference<IEnemy> enemyReference in this.spottedEnemies)
		{
			if (enemyReference != null && enemyReference.TryGetTarget(out IEnemy enemy))
			{
				if (!enemy.IsDead())
					enemy.TakeDamage(this.damagePerHit);
			}
		}
	}
}
