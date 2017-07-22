/* *************************************************
*  Created:  7/22/2017, 3:45:15 PM
*  File:     Weapon.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {

	public class Weapon : MonoBehaviour {

		// Use this for initialization
		public float force = 100;
		AudioSource effectSoundFire; 
		void Start () {
			effectSoundFire = GetComponent<AudioSource>();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public bool Shoot(Bullet bullet, ObjectProperty owner, Transform eyesTransform) {

			Debug.Assert(null != bullet,"bullet is null!!");
			Bullet clone = bullet.Instanciate<Bullet>();
			clone.transform.position = transform.position;
			clone.transform.rotation = transform.rotation;

			int maskLayer = Facade_NavMesh.GetLayerMask(
				LayerId.NonPlayer,
				LayerId.Geometry,
				LayerId.Obstacle); 
			// current look direction default
			Vector3 lookDir = transform.TransformDirection(Vector3.forward);

			// Camera direction default
			if(eyesTransform) {
				Debug.Assert(null != eyesTransform,"look object is null");
				lookDir = eyesTransform.forward;
			}

			//	Adjust aim more precisly 
			// RaycastHit hit;
			// if(Physics.Raycast(transform.position, lookDir, out hit, 1000 ,maskLayer)) {
			// 	Vector3 target = transform.position + (lookDir * hit.distance);
			// 	lookDir = (target - transform.position).normalized;
			// }
			
			if(clone.OnFire(owner,lookDir,force)) {
				if(effectSoundFire)
					effectSoundFire.Play();
			}

			return true;
		}
	}
}
