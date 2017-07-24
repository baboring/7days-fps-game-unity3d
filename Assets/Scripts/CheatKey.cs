/* *************************************************
*  Created:  7/22/2017, 11:16:34 PM
*  File:     CheatKey.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using SB;

public class CheatKey : ManualSingletonMB<CheatKey> {

	// cheat on/off
	public bool isCheatOn = false;
	public bool isDebugMode = false;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {

		// cheat state
		if(!isCheatOn)
			return;

		// Cheat spawn
		if(Input.GetKeyDown(KeyCode.Tab)) 
			SpawnManager.instance.Spawn(eSpawn.NonPlayer);

		if(Input.GetKeyDown(KeyCode.Alpha1)) {

			// Vector3 lookDir = PlayerController.instance.transform.TransformDirection(Vector3.forward);
			Vector3 lookDir = PlayerController.instance.player.eyesTransform.forward;
			float range = PlayerController.instance.property.tb.sightRange;
			SpawnManager.instance.Spawn(eSpawn.NonPlayer,PlayerController.instance.transform.position + lookDir * range);
		} 
		
		// all die
		if(Input.GetKeyDown(KeyCode.Minus)) {
			SpawnManager.instance.KillAll();
		}
	}
}
