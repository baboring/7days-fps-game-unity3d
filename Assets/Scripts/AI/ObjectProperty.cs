/* *************************************************
*  Created:  7/20/2017, 11:55:05 PM
*  File:     ObjectProperty.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace SB {

	public enum AliasType {
		None = -1,
		NonPlayer = 0,
		PlayerGroup1,
		PlayerGroup2,
	}

	public enum ObjectType {
		None 		= 0,
		Bullet 		= 10,
		MeleeWeapon = 11,
		Human 		= 100,
		Zombie		= 101,
		Robot		= 102,
	}
	public class ObjectProperty : PooledObject {

		[System.NonSerialized]
		public Unit owner;

		public R.PropertyInfo.eID tableId;
		// types
		public AliasType ally;
		public ObjectType type;

		// maguring values
		public R.PropertyInfo info { get; private set;}

		// dynmic data
		public float life = 10;

		[System.NonSerialized]
		public bool isAlive = true;

		void Awake() {
			// attach default owner
			owner = this.GetComponent<Unit>();
			Reset();
		}

		public bool Reset() {
			//Debug.Log("Property Reset - " + tableId);
			var info = R.GetPropertyInfo(tableId);
			Debug.Assert(null != info,"Not exist table id : "+tableId);
			this.info = info;

			// variable reset
			life = info.life;
			isAlive = true;
			
			return true;
		}

	}
}