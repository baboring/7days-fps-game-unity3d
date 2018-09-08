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

		// first time loaded
		public void ReadyToPlay() {
            isShowMenu = true;
            Debug.Log("Loading Start");

            Color color = new Color(0, 0, 0, 1);
            Facade_Coroutine.While(this,() => {
                if (color.a > 0) {
                    color.a -= ((Time.deltaTime > 0)? Time.deltaTime: 0.03f);
                    imgLoading.color = color;
                    return true;
                }
                Debug.Log("Loading Done");
                imgLoading.enabled = false;
                return false;
            });
            //imgLoading.color = new Color(0, 0, 0, 0);
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
		 bool isInitialized {
			 get { return PlayerController.instance != null;}
		 }

		// Game Start
		public void StartGame() {
			// all clear
			GameData.instance.Reset();
			isShowMenu = false;
       }


		public void PauseGame() {
			isShowMenu = true;
		}

		public void ResumeGame() {
			if(GameData.instance.IsGameOver)
				return;
			if(!isInitialized) {
				StartGame();
				return;
			}
			isShowMenu = false;
		}


		public void GameOver() {
			Debug.Log("GameOver");
			GameData.instance.IsGameOver = true;
			GameData.instance.IsShowGameOver = true;
			PauseGame();
		}
		
		public void ReturnToMenu() {
			GameData.instance.IsShowGameOver = false;
			isShowMenu = true;
		}
		// Update is called once per frame
		void Update () {
			if(Input.GetKeyDown(KeyCode.Escape)) {

				if(GameData.instance.IsShowGameOver) {
					GameData.instance.IsShowGameOver = false;
					return;
				}
				isShowMenu = !isShowMenu;
			}

			// if(GameData.instance.IsGameOver){

			// }
			
		}
	}
}