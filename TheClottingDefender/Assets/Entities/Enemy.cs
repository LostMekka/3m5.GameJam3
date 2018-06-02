using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public int Health = 100;
	private int CurrentHealth;
	public int RegenerationRate = 0;
	
	// Use this for initialization
	void Start ()
	{
		CurrentHealth = Health;
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentHealth < Health)
		{
			CurrentHealth += RegenerationRate;
		}
	}

	public void TakeDamage(int dmg)
	{
		CurrentHealth -= dmg;
		if (CurrentHealth <= 0)
		{
			Destroy(gameObject);
		}
	}
}
