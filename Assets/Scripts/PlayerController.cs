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
	[SerializeField] private RangeTrigger attackTrigger;

	[Header("Movement")] 
	[SerializeField] private float movementSpeed;
	[SerializeField] private float gravity;
	[SerializeField] private float rotationSpeed;

	[Header("Animations")] 
	[SerializeField] private TheDeathAnimator animator;
	
	private CharacterController characterController;

	private void Awake()
	{
		this.characterController = this.gameObject.GetComponent<CharacterController>();
	}

	private void OnEnable()
	{
		if (this.attackTrigger != null)
		{
			this.attackTrigger.onDetection += this.OnAttackRangeEnter;
		}
	}

	private void OnDisable()
	{
		if (this.attackTrigger != null)
		{
			this.attackTrigger.onDetection -= this.OnAttackRangeEnter;
		}
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
		}
	}

	private void OnAttackRangeEnter(Collider[] others)
	{
		foreach (Collider other in others)
		{
			IEnemy enemy = other.GetComponent<IEnemy>();

			if (enemy != null && !enemy.IsDead())
			{
				enemy.TakeDamage(this.damagePerHit);
			}
		}
	}
}