/* *************************************************
*  Created:  7/24/2017, 2:36:03 AM
*  File:     SoundManager.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {

	public enum eSoundId {
		Hit = 0,
		Hit2,
		Die_Hunter,
		AcquiredItem,
	}
	public class SoundManager : ManualSingletonMB<SoundManager> {

		public PooledObject[] objPrefabSounds;

		// Use this for initialization
		void Awake() {
			instance = this;
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Play(eSoundId id, Vector3 pos) {
			PooledObject snd = objPrefabSounds[(int)id].Instanciate<PooledObject>();
			((SoundChunk)snd).Play(pos);
			snd.transform.gameObject.SetActive(true);
		}
		public void Play(eSoundId id, Transform target) {
			PooledObject snd = objPrefabSounds[(int)id].Instanciate<PooledObject>();
			((SoundChunk)snd).Play(target);
			snd.transform.gameObject.SetActive(true);
		}
	}
}