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

		public AliasType type;
		public float sightRange = 15;		// range of detect target 
		public float stepAngle = 5;
		public float moveSpeed = 5;
		public float runSpeed = 5;
		public float eyeLevel = 1.5f;
		public float wander_range = 3f;

	}
}