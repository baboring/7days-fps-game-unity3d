using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {

	[RequireComponent(typeof(Collider))]
	public class Bullet : PooledObject {

		// Use this for initialization
		float force = 1;
		Vector3 direction;
		bool isHit = false;

		UnitProperty owner;
		void Awake() {

		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void FixedUpdate()
		{
			if(isHit)
				return;
			int maskLayer = Facade_NavMesh.GetLayerMask(
				LayerId.NonPlayer,
				LayerId.Geometry,
				LayerId.Obstacle); 
			// Vector3 fwd = transform.TransformDirection(Vector3.forward);
			RaycastHit hit;
			if(Physics.Raycast(transform.position, direction, out hit, force * Time.fixedDeltaTime,maskLayer)) {
				transform.position += (direction * hit.distance);
				OnTriggerEnter(hit.collider);
			}
			else 
				transform.position += (direction * force * Time.fixedDeltaTime);
		}

		// Fire
		public void OnFire(UnitProperty owner, Vector3 dir, float force ) {
			this.owner = owner;
			this.force = force;
			this.direction = dir;
			isHit = false;
		}

		void OnTriggerEnter(Collider col)
		{
			UnitProperty hitUnit = col.gameObject.GetComponent<UnitProperty>();
			
			Debug.Assert(null != this.owner,"Bullet's Owner is null");
			Debug.Log("hit-:" + col.gameObject.name);
			isHit = true;
			
			Facade_Coroutine.DelaySeconds(this,()=>{
				this.ReturnToPool();
			},3);			
			
			//all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
			if (null != hitUnit && hitUnit.ally != this.owner.ally )
			{
				// Destroy(col.gameObject);
				// //add an explosion or something
				// //destroy the projectile that just caused the trigger collision
				// Destroy(gameObject);
				PooledObject obj =  hitUnit.gameObject.GetComponent<PooledObject>();
				if(obj)
					obj.ReturnToPool();
			}
		}
	}
}