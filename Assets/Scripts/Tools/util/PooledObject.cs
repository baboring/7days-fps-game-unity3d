﻿/* *************************************************
*  Created:  7/20/2017, 2:05:05 PM
*  File:     PooledObject.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using UnityEngine;

namespace SB {
	public class PooledObject : MonoBehaviour {

		[System.NonSerialized]
		ObjectPool poolInstanceForPrefab;

		public T Instanciate<T> () where T : PooledObject {
			if (!poolInstanceForPrefab) {
				poolInstanceForPrefab = ObjectPool.CreateObjectPool(this);
			}
			return (T)poolInstanceForPrefab.GetObject();
		}

		public ObjectPool poolHandler { get; set; }

		public void ReturnToPool () {
			if (poolHandler) {
				poolHandler.AddObject(this);
			}
			else {
				Debug.Log("I die!");
				Destroy(gameObject);
			}
		}
	}
}