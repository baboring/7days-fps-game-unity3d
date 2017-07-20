using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickRayCastPoint : MonoBehaviour {

	void LateUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			// create a ray from your mouse-position
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(r, out hit)){
				// if the ray hit a collider, we'll get the world-coordinate here.
				Vector3 worldPos = hit.point;

				// var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				// go.transform.position = hit.point;
				Debug.DrawRay (r.origin, r.direction * hit.distance, Color.green);
			}
			else
				Debug.DrawRay (r.origin, r.direction * 10, Color.red);		
	
		}
	}
		
}
