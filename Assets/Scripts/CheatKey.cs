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
		
		// all die
		if(Input.GetKeyDown(KeyCode.Minus)) {
			foreach(var obj in SpawnManager.instance.All.ToArray()) {
				if(null != obj.owner)
					obj.owner.OnDie(null);
			}
		}
	}
}
