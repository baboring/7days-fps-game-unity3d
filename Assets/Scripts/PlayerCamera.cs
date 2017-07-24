/* *************************************************
*  Created:  7/22/2017, 12:39:28 AM
*  File:     PlayerCamera.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using UnityEngine;

namespace SB {


	[RequireComponent(typeof(SetTargetLookFollow))]
	public class PlayerCamera : MonoBehaviour {

		public Transform target;
		public Vector3 offset = new Vector3(0f, 7.5f, 0f);

		// Use this for initialization
		void Start () {
			
		}
		
		public void AttachTo(Unit player, Vector3 offset = default(Vector3)) {

			Debug.Assert(null != player,"player controller is null");
			if(!player)
				return;
			target = player.transform;
			if(offset != default(Vector3))
				this.offset = offset;

			// attach to body direction handler 
			SetTargetLookFollow script = GetComponent<SetTargetLookFollow>();
			if(script)
				script.target = player.transform;
			player.ConnectToEyes(this.transform);
			Camera playerCam = GetComponent<Camera>();
			if(playerCam) {
				Debug.Log("switch camera");
                if(Camera.main)
    				Camera.main.enabled = false;
				playerCam.enabled = true;
			}
			
		}


		private void LateUpdate() {

			// fowllow
			FollowTarget();
		}

		void FollowTarget() {
			if(target)
				transform.position = target.position + offset;
		}
		
	}
}