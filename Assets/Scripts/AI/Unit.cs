/* *************************************************
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
		// Update is called once per frame
		void Update () {
			
		}
		
		public bool IsAlive { get {return property.isAlive;} }
		abstract public void OnDamage(ObjectProperty uInfo);
		abstract public void OnDie(ObjectProperty uInfo);

		public void DisposeForPool() {

			// Just in case, remove this again from SpawnManager
			SpawnManager.instance.Remove(property);
			
			property.ReturnToPool();
		}
	}
}