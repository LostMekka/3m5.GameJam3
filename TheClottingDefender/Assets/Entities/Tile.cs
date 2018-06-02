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
		SelectionBox.transform.position = Vector3.zero;
		SelectionBox.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnMouseOver()
	{
		Debug.Log("mouse hover");
		SelectionBox.gameObject.SetActive(true);
	}

	void OnMouseExit()
	{
		Debug.Log("mouse exit");
		SelectionBox.gameObject.SetActive(false);
	}
}