/* *************************************************
*  Created:  7/22/2017, 12:07:26 AM
*  File:     GameManager.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {


	public class GameManager : ManualSingletonMB<GameManager> {

		void Awake() {
			instance = this;
		}
		// Use this for initialization
		void Start() {
			Debug.Log("GameManager initialized");
		}

		public void StartGame() {
			// Spawn Player
			Debug.Log("----- StartGame --------");
			GameObject[] playerCameras = GameObject.FindGameObjectsWithTag("Player Camera");
			PlayerController player = SpawnManager.instance.RandomSpawn(eSpawn.Player) as PlayerController;
			foreach(var go in playerCameras) {
				if(go.GetComponent<PlayerCamera>()) {
					go.GetComponent<PlayerCamera>().AttachTo(player);
				}
			}

		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}