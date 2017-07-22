/* *************************************************
*  Created:  7/20/2017, 2:04:07 PM
*  File:     SpawnManager.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SB {
	public class SpawnManager : MonoBehaviour {

		public Transform[] spots;
		public Unit[] objPrefab;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown(KeyCode.Tab)) {
				Vector3 point;
				if (Facade_NavMesh.RandomPoint(spots[Random.Range(0, spots.Length)].position, 10f, out point)) {
					SpawnObject(objPrefab[Random.Range(0, objPrefab.Length)],point);
				}
			}
		}

		void SpawnObject(Unit prefab, Vector3 pos) {
			Debug.DrawRay(pos, Vector3.up, Color.blue, 5.0f);

			Unit spawn = prefab.Instanciate<Unit>();
			spawn.transform.position = pos;

			// var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			// go.transform.position = pos;
		}
	}
}