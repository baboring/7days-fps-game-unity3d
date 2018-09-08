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
		Max
	}
	public class SpawnManager : ManualSingletonMB<SpawnManager> {

		public int MaxSpawnNum = 30;
		public float SecondsForSpawn = 2;

		public Transform[] spots;
		public PooledObject[] objPrefabNPC;
		public PooledObject[] objPrefabPlayer;

		public List<ObjectProperty> All = new List<ObjectProperty>();

		// Use this for initialization
		void Awake() {
			instance = this;
		}
		void Start() {
			Debug.Log("SpawnManager initialized");
			// gradually increse ememies.
			// InvokeRepeating("OnSpawnCheck", 1.0f, SecondsForSpawn);
		}


		// gradually increse ememies.
		void OnSpawnCheck() {
			Debug.Log("OnSpawnCheck:" + All.Count);
			// Debug Mode
			if(CheatKey.instance.isDebugMode || GameData.instance.IsPause)
				return;			
			if(All.Count < MaxSpawnNum)
				Spawn(eSpawn.NonPlayer);
		}
		// Update is called once per frame
		float elpasedTime = 0;
		void Update () {
			if(elpasedTime > SecondsForSpawn) {
				elpasedTime = 0;
				OnSpawnCheck();
			}
			elpasedTime += Time.deltaTime;
			// for dev log display
			HUDDevDisplay.instance.Show("Spawns",All.Count.ToString()); 
		}

		public void Remove(ObjectProperty obj) {
			All.Remove(obj);
		}

		// Spawn unit
		PooledObject SpawnObject(PooledObject prefab, Vector3 pos) {
			Debug.DrawRay(pos, Vector3.up, Color.blue, 5.0f);

			PooledObject spawn = prefab.Instanciate<PooledObject>();
			spawn.transform.position = pos;
			spawn.transform.gameObject.SetActive(true);

			// var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
			// go.transform.position = pos;
			return spawn;
		}

		// spawn randowm object in type
		public PooledObject Spawn(eSpawn type, Vector3 initPos = default(Vector3)) {
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
			if (objPrefab != null && posPrefab != null) { 
				const int tryCnt = 5;
				if(initPos == default(Vector3)) {
					for(int i=0;i<tryCnt;++i) {
						if(Facade_NavMesh.RandomRangePoint(posPrefab[index].position, 0, 10f, out initPos))
							break;
					}
				}

				// fails to found the position
				if(initPos == default(Vector3))
					return null;

				var objSpawn = SpawnObject(objPrefab[Random.Range(0, objPrefab.Length)],initPos);
				// save for searching in AI
				ObjectProperty property = objSpawn as ObjectProperty;
				if(null != property) {
					All.Add(property);
				}
				return objSpawn;
			}

			return null;
		}

        public PooledObject Respawn(PooledObject spawnedObj) { 
            foreach(var prefab in objPrefabPlayer) {
                if(prefab.instance == spawnedObj.poolHandler) {
                    return prefab.Instanciate<PooledObject>(spawnedObj);
                }
            }
            return spawnedObj;
        }

		public Transform FindRandomSpawnSpot(eSpawn type) {
			return spots[Random.Range(0, spots.Length)];
		}

        // find spot in spawn spots
        public Transform FindFurthestSpot(ObjectProperty obj) {
            Vector3 longest = Vector3.zero;
            Transform pick = null;
            foreach (var t in spots) {
                Vector3 v = obj.transform.DistanceFrom(t);
                if(v.magnitude > longest.magnitude) {
                    longest = v;
                    pick = t;
                }
            }

            return pick;
        }

		public void KillAll() {
			foreach(var obj in All.ToArray()) {
				if(null != obj.owner)
					obj.owner.OnDie(null);
			}
		}
		
	}
}