using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SB {

	public enum LayerId {
		PlayerOwner = 8,
		PlayerOther = 9,
		NonPlayer = 10,		
		Geometry = 14,
		Obstacle = 15,
	}
	public static class Facade_NavMesh {

		public static bool RandomPoint(Vector3 center, float range, out Vector3 result) {
			for (int i = 0; i < 30; i++) {
				Vector3 randomPoint = center + Random.insideUnitSphere * range;
				NavMeshHit hit;
				if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
					result = hit.position;
					return true;
				}
			}
			result = Vector3.zero;
			return false;
		}

		// Make Layer Mask 
		public static int GetLayerMask(params LayerId[] layers) {
			int mask = 0;
			foreach(LayerId layer in layers) {
				mask += (1 << (int)layer);
			}
			return mask;
		} 
	}

}