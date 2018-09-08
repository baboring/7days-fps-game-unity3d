/* *************************************************
*  Created:  7/24/2017, 2:11:24 AM
*  File:     SoundChunk.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {

	public class SoundChunk : PooledObject {

		// Use this for initialization
		AudioSource snd; 
		Transform target;
		float wait;
		bool check;		

		void Awake () {
			snd = GetComponent<AudioSource>();
		}

		// reset
		void OnEnable() {
			target = null;
			check = false;
			wait = 0;
			if(snd)
				snd.Stop();
		}

		public void Play(Vector3 pos) {
			transform.position = pos;
			Debug.Assert(null != snd,"AudioSource is null in Sound script");
			if(snd) {
                Debug.Assert(null != snd.clip, "Audio Clip is Empty!!!");
                snd.Play();
                wait = snd.clip.length; //set wait to be clip's length
				check = true;
			}
		}
		public void Play(Transform target) {
			Debug.Assert(null != target,"target is null in Sound script");
			this.target = target;
			Play(target.position);

		}

		void Update() {
			// looked attached
			if(target)
				transform.position = target.position;

			if(check)
				wait-=Time.deltaTime; //reverse count
	
			if(wait<0f && check){ //here you can check if clip is not playing
				check=false;
				target = null;
				this.ReturnToPool();
			}			
		}

	}
}