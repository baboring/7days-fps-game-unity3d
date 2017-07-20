/* *************************************************
*  Created:  7/20/2017, 1:43:34 PM
*  File:     MoveToClickPoint.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    
[RequireComponent(typeof(NavMeshAgent))]
public class MoveToClickPoint : MonoBehaviour {
	NavMeshAgent agent;
    public Transform markerObject;
	
	void Start() {
		agent = GetComponent<NavMeshAgent>();
	}
	
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				agent.destination = hit.point;
				if(markerObject)
					markerObject.position = hit.point;
			}
		}
	}
}