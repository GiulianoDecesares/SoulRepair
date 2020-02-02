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
	[Header("Attack")] [SerializeField] private KeyCode attack;
	[SerializeField] private float damagePerHit;

	[Header("Movement")] [SerializeField] private float movementSpeed;
	[SerializeField] private float gravity;
	[SerializeField] private float rotationSpeed;

	[Header("Animations")] 
	[SerializeField] private TheDeathAnimator animator;
	
	private CharacterController characterController;

	private List<WeakReference<IEnemy>> spottedEnemies;

	private void Awake()
	{
		this.characterController = this.gameObject.GetComponent<CharacterController>();
		this.spottedEnemies = new List<WeakReference<IEnemy>>();
	}

	private void Update()
	{
		Vector3 rotation = new Vector3(0, Input.GetAxisRaw("Horizontal") * this.rotationSpeed * Time.deltaTime, 0);
		Vector3 move = new Vector3(0, 0, Input.GetAxisRaw("Vertical") * Time.deltaTime);

		move = this.transform.TransformDirection(move);
		
		this.characterController.Move(move * this.movementSpeed);
		this.transform.Rotate(rotation);

		// Manage animations
		this.animator.Walk(rotation.magnitude + move.magnitude);

		if (Input.GetKeyDown(this.attack))
		{
			this.animator.Slash();
			this.Attack();
		}
		else if (Input.GetKeyUp(this.attack))
		{
			
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