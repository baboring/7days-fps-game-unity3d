using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SB;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		R.Create();		
		Facade_Coroutine.Sequnce(this,
            Facade_Coroutine.Wait(() => {
                // start at Map1
                // if(!GameData.instance) {
                // 	GameData.instance = Singleton<GameData>.Instantiate();
                // }
                return !GameData.instance;
            }),
            Facade_Coroutine.Wait(() => {return !SpawnManager.instance;}),
            Facade_Coroutine.Wait(() => {return !GameManager.instance;}),
			Facade_Coroutine.Do(()=>{
				GameManager.instance.StartGame();
			})
		);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
