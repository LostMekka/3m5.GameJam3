using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public Selection SelectionPrefab;
	public Floor FloorPrefab;
	public Tower TowerPrefab;

	private SpriteRenderer SpriteRenderer;
	private Floor Floor;
	private Tower Tower;
	private Selection SelectionBox;

	// Use this for initialization
	void Start()
	{
		SelectionBox = Instantiate(SelectionPrefab);
		SelectionBox.transform.parent = transform;
		SelectionBox.transform.localPosition = Vector3.zero;
		SelectionBox.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnMouseOver()
	{
		SelectionBox.gameObject.SetActive(true);
	}

	void OnMouseExit()
	{
		SelectionBox.gameObject.SetActive(false);
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