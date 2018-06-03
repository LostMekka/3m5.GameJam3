using Entities;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEnemyEvent : UnityEvent<Enemy>
{
}

public class Enemy : MonoBehaviour
{
	public int MaxHealth = 100;
	public int RegenerationRate;
	public float MaxMoveSpeed = 1f;
	public int Bounty = 5;
	public MyEnemyEvent dieEvent = new MyEnemyEvent();
	public Level ParentLevel;

	private int currentHealth;
	private Rigidbody2D body;
	private Tile currentTile;
	private Vector2 targetMoveDirection = Vector2.right;



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
			dieEvent.Invoke(this);
			Destroy(gameObject);
			
		}
	}
}