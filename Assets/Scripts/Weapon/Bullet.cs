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

		ObjectProperty owner;
		void OnEnable() {
			isHit = false;
			owner = null;
		}
		
		// Update is called once per frame
		void FixedUpdate()
		{
			if(isHit)
				return;
			int maskLayer = Facade_NavMesh.GetLayerMask(
				((owner.ally == AliasType.NonPlayer)? LayerId.PlayerAllias : LayerId.NonPlayer),
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
		public bool OnFire(ObjectProperty owner, Vector3 dir, float force ) {
			this.owner = owner;
			this.force = force;
			this.direction = dir;
			isHit = false;

			return true;
		}

		void OnTriggerEnter(Collider col)
		{
			ObjectProperty hitUnit = col.gameObject.GetComponent<ObjectProperty>();
			
			Debug.Assert(null != this.owner,"Bullet's Owner is null");
			isHit = true;
	
			
			//all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
			if (null != hitUnit && hitUnit.ally != this.owner.ally )
			{
				//Debug.Log("hit-:" + col.gameObject.name);
				// Destroy(col.gameObject);
				// //add an explosion or something
				// //destroy the projectile that just caused the trigger collision
				// Destroy(gameObject);
				Unit obj =  hitUnit.gameObject.GetComponent<Unit>();
				if(obj && obj.IsAlive) {
					obj.OnDamage(owner);
				}
				this.ReturnToPool();
			}
			else {
				Facade_Coroutine.DelaySeconds(this,()=>{
					this.ReturnToPool();
				},3);	
			}
		}
	}
}