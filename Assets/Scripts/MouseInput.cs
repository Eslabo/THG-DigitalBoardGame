using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{

	public Camera camera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D rch2d = Physics2D.Raycast(ray.origin, ray.direction);

			if (rch2d)
			{
				Debug.Log(rch2d.collider.gameObject.name, rch2d.collider.gameObject);
			}
		}
	}
}
