using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public int MaxHealth = 100;
	public int RegenerationRate;
	public float MaxMoveSpeed = 1f;

	private int currentHealth;
	private Rigidbody2D body;

	// Use this for initialization
	void Start()
	{
		currentHealth = MaxHealth;
		body = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		RegenerateHealth();
		AddMovement();
	}

	private void RegenerateHealth()
	{
		if (currentHealth < MaxHealth) currentHealth += RegenerationRate;
	}

	private void AddMovement()
	{
		if (body.velocity.sqrMagnitude < MaxMoveSpeed * MaxMoveSpeed) body.AddForce(Vector2.right);
	}

	public void TakeDamage(int dmg)
	{
		currentHealth -= dmg;
		if (currentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}
}