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
	public int Bounty = 5;
	public int RegenerationRate;
	public float MaxMoveSpeed = 2f;
	public float SteeringForce = 10f;
	public float AccelerationForce = 8f;
	public Level ParentLevel;

	public MyEnemyEvent dieEvent = new MyEnemyEvent();

	private int currentHealth;
	private Rigidbody2D body;
	private Tile currentTile;
	private Vector2 targetMoveDirection = Vector2.right;
	private RegularTimer directionUpdateTimer;

	// Use this for initialization
	void Start()
	{
		currentHealth = MaxHealth;
		body = GetComponent<Rigidbody2D>();
		directionUpdateTimer = new RegularTimer(1f);
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
		if (ParentLevel != null)
		{
			Tile t = ParentLevel.GetTileForGlobalPosition(transform.position.x, transform.position.y);
			if (t != null)
			{
				if (t != currentTile || directionUpdateTimer.CheckElapsedAndAutoRewind())
				{
					Vector2[] flow = ParentLevel.GetFlowAt(t.X, t.Y);
					if (flow == null || flow.Length == 0)
					{
						targetMoveDirection = Vector2.right;
					}
					else
					{
						targetMoveDirection = flow[(int) (Random.value * flow.Length)];
					}
				}
			}

			currentTile = t;
		}

		float angleDiff = Vector2.Angle(targetMoveDirection, body.velocity);
		Vector2 velDiff = Normalize(targetMoveDirection) - Normalize(body.velocity.normalized);
		if (angleDiff > 0.1f) body.AddForce(Normalize(velDiff) * SteeringForce);
		if (body.velocity.sqrMagnitude > MaxMoveSpeed * MaxMoveSpeed + 0.1f) body.AddForce(-body.velocity * AccelerationForce);
		if (body.velocity.sqrMagnitude < MaxMoveSpeed * MaxMoveSpeed - 0.1f) body.AddForce(body.velocity * AccelerationForce);
	}

	private Vector2 Normalize(Vector2 vec)
	{
		return vec.Equals(Vector2.zero)
			? Vector2.zero
			: vec.normalized;
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