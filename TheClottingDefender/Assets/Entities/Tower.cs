using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

	public float Cooldown = 5f;
	public int Health = 200;
	public int Damage = 100;
	public int Range = 3;
	public float BulletSpeed = 1.0f;
	public Bullet BulletPrefab;
	
	private float NextTimeFiring;

	// Use this for initialization
	void Start ()
	{
		NextTimeFiring = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (NextTimeFiring <= Time.time)
		{
			Enemy target = getNearestTarget();
			if (target != null)
			{
				Shoot(target);
			}
		}
	}

	private void Shoot(Enemy target)
	{
		Bullet bullet = Instantiate(BulletPrefab);
		bullet.Target = target;
		bullet.Damage = Damage;
		bullet.Speed = BulletSpeed;
		bullet.transform.position = transform.position;
		NextTimeFiring = Time.time + Cooldown;
	}

	private Enemy getNearestTarget()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length > 0)
		{
			Enemy closest = null;
			float distance = Mathf.Infinity;
			Vector2 position = transform.position;
			foreach (GameObject go in enemies)
			{
				Vector2 diff = (Vector2) go.transform.position - position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < Range && curDistance < distance)
				{
					closest = go.GetComponent<Enemy>() ?? closest;
					distance = curDistance;
				}
			}

			return closest;
		}
		return null;
	}
}
