﻿/* *************************************************
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
		public float sightRange  { get; private set;}		// range of detect target 
		public float stepAngle  { get; private set;}

		public float eyeLevel  { get; private set;}
		public float wander_range  { get; private set;}


		// basic ability ( constant )
		public float walkSpeed  { get; private set;}
		public float runSpeed { get; private set;}
		public float acceleration { get; private set;}
		public float angular_speed { get; private set;}
		public float attack_power { get; private set;}

		// dynmic data
		public float life = 10;


		void Awake() {
			// attach default owner
			owner = this.GetComponent<Unit>();
			Reset();
		}

		public bool Reset() {
			//Debug.Log("Property Reset - " + tableId);
			var info = R.GetPropertyInfo(tableId);
			Debug.Assert(null != info,"Not exist table id : "+tableId);
			attack_power = info.attack_power;
			sightRange = info.sightRange;
			eyeLevel = info.eyeLevel;
			wander_range = info.wander_range;
			walkSpeed = info.walkSpeed;
			runSpeed = info.runSpeed;
			acceleration = info.acceleration;
			angular_speed = info.angular_speed;

			life = info.life;
			
			return true;
		}

	}
}