using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	private SpriteRenderer SpriteRenderer;
	public Floor FloorPrefab;
	private Floor Floor;
	public Tower TowerPrefab;
	private Tower Tower;

	// Use this for initialization
	void Start () {
		SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseOver()
	{
		SpriteRenderer.enabled = true;
	}

	void OnMouseExit()
	{
		SpriteRenderer.enabled = false;
	}

	private void OnMouseDown()
	{
		if (!BuildFloor() && !BuildTower())
		{
			Destroy(Floor);
			Destroy(Tower);
		}
		
	}

	bool BuildFloor()
	{
		if (Floor != null) return false;
		Floor = Instantiate(FloorPrefab);
		Floor.transform.position = transform.position;
		Floor.transform.parent = transform;
		return true;

		
	}

	bool BuildTower()
	{
		if (Floor == null || Tower != null) return false;
		Tower = Instantiate(TowerPrefab);
		Tower.transform.position = transform.position;
		Tower.transform.parent = transform;
		return true;
	}
}
