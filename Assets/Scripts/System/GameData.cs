using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ManualSingletonMB<GameData> {

	void Awake() {
		instance = this;
	}
	// Use this for initialization
	void Start () {
		Debug.Log("GameData initialized");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
