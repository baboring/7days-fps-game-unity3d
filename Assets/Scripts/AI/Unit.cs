﻿/* *************************************************
*  Created:  7/20/2017, 2:04:16 PM
*  File:     Unit.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SB {

	[RequireComponent(typeof(ObjectProperty))]
	public abstract class Unit : MonoBehaviour {

        [System.NonSerialized]
		public ObjectProperty property;
		// Use this for initialization

		protected void Awake () {
			property = GetComponent<ObjectProperty>();
		}

		protected void OnEnable() {
			Debug.Assert(null != property,"Unit's propery is null");
			property.Reset();
		}
		
		public bool IsAlive { get {return property.isAlive;} }

		abstract public void OnAttack(ObjectProperty obj);
		abstract public void OnDamage(ObjectProperty skill);
		abstract public void OnDie(ObjectProperty skill);

		public void DisposeForPool() {

			// Just in case, remove this again from SpawnManager
			SpawnManager.instance.Remove(property);
			
			property.ReturnToPool();
		}

        [System.NonSerialized]
        public Transform eyesTransform;     // for eye level
        public void ConnectToEyes(Transform cam)
        {
            if (cam)
                eyesTransform = cam;
        }


    }
}