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

	public enum eSpawn {
		Player = 0,
		NonPlayer,
		Item,
	}
	public class SpawnManager : ManualSingletonMB<SpawnManager> {

		public Transform[] spots;
		public PooledObject[] objPrefabNPC;
		public PooledObject[] objPrefabPlayer;
		// Use this for initialization
		void Awake() {
			instance = this;
		}
		void Start() {
			Debug.Log("SpawnManager initialized");
		}
		public PooledObject RandomSpawn(eSpawn type) {
			Vector3 point;
			PooledObject[] objPrefab = null;
			Transform[] posPrefab = null;
			switch(type) {
				case eSpawn.Player:
					objPrefab = objPrefabPlayer;
					posPrefab = spots;
					break;
				case eSpawn.NonPlayer:
					objPrefab = objPrefabNPC;
					posPrefab = spots;
					break;
			}
			int index = Random.Range(0, posPrefab.Length);

			if (objPrefab != null && posPrefab != null && Facade_NavMesh.RandomRangePoint(posPrefab[index].position, 0, 10f, out point)) {
				return SpawnObject(objPrefab[Random.Range(0, objPrefab.Length)],point);
			}
			return default(PooledObject);
		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown(KeyCode.Tab)) {
				Vector3 point;
				int index = Random.Range(0, spots.Length);
				if (Facade_NavMesh.RandomRangePoint(spots[index].position, 0, 10f, out point)) {
					SpawnObject(objPrefabNPC[Random.Range(0, objPrefabNPC.Length)],point);
				}
			}
		}

		PooledObject SpawnObject(PooledObject prefab, Vector3 pos) {
			Debug.DrawRay(pos, Vector3.up, Color.blue, 5.0f);

			PooledObject spawn = prefab.Instanciate<PooledObject>();
			spawn.transform.position = pos;
			spawn.transform.gameObject.SetActive(true);

			// var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			// go.transform.position = pos;
			return spawn;
		}
	}
}