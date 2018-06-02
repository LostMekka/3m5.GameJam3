using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	private SpriteRenderer SpriteRenderer;

	// Use this for initialization
	void Start () {
		Debug.Log("Starting");
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
}
