using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public Selection SelectionPrefab;

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
}