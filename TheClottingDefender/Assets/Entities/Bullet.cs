using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public Enemy target;
	public float speed = 0.5f;
	public int Damage = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, step);

		if ((transform.position - target.gameObject.transform.position).sqrMagnitude < 0.01f)
		{
			Destroy(gameObject);
			target.TakeDamage(Damage);
		}
	}
}
