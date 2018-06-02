using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public Enemy Target;
	public float Speed = 0.5f;
	public int Damage = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Target != null)
		{
			float step = Speed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, Target.gameObject.transform.position, step);

			if ((transform.position - Target.gameObject.transform.position).sqrMagnitude < 0.01f)
			{
				Destroy(gameObject);
				Target.TakeDamage(Damage);
			}
		}
		else
		{
			Destroy(gameObject);
		}
		
	}
}
