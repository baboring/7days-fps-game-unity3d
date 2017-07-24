/* *************************************************
*  Created:  7/22/2017, 12:07:26 AM
*  File:     GameManager.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SB {


	public class GameManager : ManualSingletonMB<GameManager> {

		public GameObject HUD_MainMenu;
		public GameObject HUD_Loading;

		private Image  imgLoading;
		void Awake() {
			instance = this;
			imgLoading = HUD_Loading.GetComponent<Image>();
			imgLoading.color = new Color(0,0,0,1);
		}
		// Use this for initialization
		void Start() {
			
			Debug.Log("GameManager initialized");
		}

		public void ReadyToPlay() {
            isShowMenu = true;
            Debug.Log("Loading Start");

            Color color = new Color(0, 0, 0, 1);
            Facade_Coroutine.While(this,() => {
                if (color.a > 0) {
                    color.a -= ((Time.deltaTime > 0)? Time.deltaTime: 0.03f);
                    imgLoading.color = color;
					//Debug.Log(color.a.ToString("0.0"));
                    return true;
                }
                Debug.Log("Loading Done");
                imgLoading.enabled = false;
                return false;
            });
            //imgLoading.color = new Color(0, 0, 0, 0);
 		}

		 bool isInitialized {
			 get { return PlayerController.instance != null;}
		 }

		public void StartGame() {
			isShowMenu = false;
			// all clear
			SpawnManager.instance.KillAll();
 			
			// Spawn Player
			Debug.Log("----- StartGame --------");
			GameObject[] playerCameras = GameObject.FindGameObjectsWithTag("Player Camera");
			if(PlayerController.instance) {
				var spot = SpawnManager.instance.FindRandomSpawnSpot(eSpawn.Player);
				PlayerController.instance.Respawn(spot);
            }
            else {
                var player = SpawnManager.instance.Spawn(eSpawn.Player) as ObjectProperty;
                player.transform.gameObject.AddComponent<PlayerController>();
                Debug.Assert(null != player, "Random Spawn fails : player is null");
                foreach (var go in playerCameras) {
                    if (go.GetComponent<PlayerCamera>()) {
                        go.GetComponent<PlayerCamera>().AttachTo(player.owner);
                    }
                }
            }

       }

		public bool isShowMenu {
			get{
				return HUD_MainMenu.activeSelf;
			}
			set {
				HUD_MainMenu.SetActive(value);
				GameData.instance.IsPause = value;
			}
		}

		public void PauseGame() {
			isShowMenu = true;
		}
		public void ResumeGame() {
			if(!isInitialized) {
				StartGame();
				return;
			}
			isShowMenu = false;
		}

		public void GameOver() {
			PauseGame();
		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				isShowMenu = !isShowMenu;
			}
			
		}
	}
}