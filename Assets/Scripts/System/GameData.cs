using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ManualSingletonMB<GameData> {

	public int Score {get; set;}
	public bool IsGameOver {get; set;}
	public float Timer {get; set;}	
	void Awake() {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		Debug.Log("GameData initialized");
	}

	public void Reset() {
		Score = 0;
		IsGameOver = false;
		IsPause = false;
		Timer = 60;
	}
	
	public bool IsPause {
		get {
			return Time.timeScale < 1;
		}
		set {
			Time.timeScale = (value)? 0 : 1;
		}
	}
	

	// Update is called once per frame
	void Update () {

		if(!IsGameOver && !IsPause)
			Timer += Time.deltaTime;
	}
}
