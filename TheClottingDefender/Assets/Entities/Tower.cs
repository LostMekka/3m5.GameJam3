using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

	private GameObject Target;
	public int Cooldown;
	private int CurrentCooldown;
	public int Health;
	public int Range;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentCooldown < 0)
		{
			CurrentCooldown--;
		}
		else
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		// #TODO: implement logic to shoot on target enemy
		CurrentCooldown = Cooldown;
	}
}
