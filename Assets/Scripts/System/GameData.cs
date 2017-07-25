using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB;

public class GameData : ManualSingletonMB<GameData> {

	public SmoothMouseLook playerCamera;
	public int Score {get; set;}
	public bool IsGameOver {get; set;}
	public bool IsShowGameOver {get; set;}
	public bool IsStartGame {get; set;}
	public float Timer {get; set;}	

	float totalTime = 60 * 10;
	void Awake() {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		Debug.Log("GameData initialized");
		Timer = totalTime;
	}

	public void Reset() {

		// initial npc
		SpawnManager.instance.KillAll();

        Score = 0;
        IsStartGame = true;
        IsGameOver = false;
        IsShowGameOver = false;
        IsPause = false;
        Timer = totalTime;

	
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

    public void AddScore(int score) {
        Score += score;
        HUDDevDisplay.instance.Show("Score:",Score.ToString());
    }
	
	public bool IsPause {
		get {
			return Time.timeScale < 1;
		}
		set {
			Time.timeScale = (value)? 0 : 1;
			playerCamera.SetCursorLock(!value);
		}
	}
	

	// Update is called once per frame
	void Update () {

		if(!IsGameOver && !IsPause && IsStartGame) {
			Timer -= Time.deltaTime;
			if(Timer <=  0) {
				Timer = 0;
				IsPause = true;
				IsGameOver = true;
				IsShowGameOver = true;
				IsStartGame = false;
			}
		}
	}
}
