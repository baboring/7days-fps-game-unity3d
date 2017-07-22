/* *************************************************
*  Created:  7/20/2017, 11:55:05 PM
*  File:     UnitProperty.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/

using UnityEngine;

namespace SB {

	public enum AliasType {
		NonPlayer = 0,
		PlayerGroup1,
		PlayerGroup2,
	};
	public class UnitProperty : MonoBehaviour {

		// types
		public AliasType ally;

		// maguring values
		public float sightRange = 15;		// range of detect target 
		public float stepAngle = 5;

		public float eyeLevel = 1.5f;
		public float wander_range = 3f;


		// basic ability 
		public float walkSpeed = 1;		// walk speed
		public float runSpeed = 2;
		public float acceleration = 2;
		public float angular_speed = 120;

	}
}