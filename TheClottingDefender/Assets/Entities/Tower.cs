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
	public Tile ParentTile;
	
	private float NextTimeFiring;

	// Use this for initialization
	private void Start ()
	{
		NextTimeFiring = Time.time;
	}
	
	// Update is called once per frame
	private void Update () {
		if (NextTimeFiring <= Time.time)
		{
			Enemy target = GetNearestTarget();
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
		ParentTile.ParentLevel.GameController.TowerShootAudio.Play();
	}

	private Enemy GetNearestTarget()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length > 0)
		{
			Enemy closest = null;
			float squareDistance = Mathf.Infinity;
			Vector2 position = transform.position;
			foreach (GameObject go in enemies)
			{
				Vector2 diff = (Vector2) go.transform.position - position;
				float currSquareDistance = diff.sqrMagnitude;
				if (currSquareDistance < Range*Range && currSquareDistance < squareDistance)
				{
					closest = go.GetComponent<Enemy>() ?? closest;
					squareDistance = currSquareDistance;
				}
			}

			return closest;
		}
		return null;
	}
}
