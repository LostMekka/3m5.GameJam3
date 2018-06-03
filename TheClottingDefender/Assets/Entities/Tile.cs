using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
	public Selection SelectionPrefab;
	public Floor FloorPrefab;
	public Tower TowerPrefab;
	public List<Tile> FlowFieldTargets;
	public int? FlowFieldDistance;
	public bool IsInteractable = true;

	public int X;
	public int Y;

	public UnityEvent IsBlockingChanged = new UnityEvent();

	private SpriteRenderer spriteRenderer;
	private Floor floor;
	private Tower tower;
	private Selection selectionBox;

	public bool IsBlocking
	{
		get { return isBlockingInternalField; }
		private set
		{
			bool changed = isBlockingInternalField != value;
			isBlockingInternalField = value;
			if (changed) IsBlockingChanged.Invoke();
		}
	}

	private bool isBlockingInternalField;

	// Use this for initialization
	private void Start()
	{
		selectionBox = Instantiate(SelectionPrefab);
		selectionBox.transform.parent = transform;
		selectionBox.transform.localPosition = Vector3.zero;
		selectionBox.gameObject.SetActive(false);
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnMouseOver()
	{
		if (!IsInteractable) return;
		selectionBox.gameObject.SetActive(true);
	}

	private void OnMouseExit()
	{
		selectionBox.gameObject.SetActive(false);
	}

	private void OnMouseDown()
	{
		if (!IsInteractable) return;
		if (BuildFloor() || BuildTower()) return;
		Destroy(floor.gameObject);
		Destroy(tower.gameObject);
		floor = null;
		tower = null;
		IsBlocking = false;
	}

	private bool BuildFloor()
	{
		if (!IsInteractable) return false;
		if (floor != null) return false;
		floor = Instantiate(FloorPrefab);
		floor.transform.position = transform.position;
		floor.transform.parent = transform;
		IsBlocking = true;
		return true;
	}

	private bool BuildTower()
	{
		if (!IsInteractable) return false;
		if (floor == null || tower != null) return false;
		tower = Instantiate(TowerPrefab);
		tower.transform.position = transform.position;
		tower.transform.parent = transform;
		return true;
	}
}