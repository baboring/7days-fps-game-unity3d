using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SB {

	[RequireComponent(typeof(NavMeshAgent))]
	public class AI_Unit : Unit {

		// weapon and bullet
		public Weapon weapon;
		public Bullet bullet;
		
		protected NavMeshAgent agent;
		protected Animator animator;

		// Use this for initialization
		protected void Awake () {
			base.Awake();
			animator = GetComponent<Animator>();
			agent = GetComponent<NavMeshAgent>();
		}

		/// <summary>
		/// This function is called when the object becomes enabled and active.
		/// </summary>
		protected void OnEnable()
		{
			base.OnEnable();
			Debug.Assert(null != agent,"navAgent is null");
			agent.ResetPath();
			// agent.Resume();

			Debug.Assert(null != animator,"animator is null");
			animator.ResetParameters();
			animator.SetTrigger("Reset");

			// default speed
			agent.speed = property.tb.walkSpeed;
			agent.angularSpeed = property.tb.angularSpeed;
			agent.stoppingDistance = property.tb.stoppingDist;
			
			//Debug.Log("Enabled 0/"+ this.GetType().Name);
							
		}
        void OnGUI() {
            Camera cam = (Camera.main) ? Camera.main : Camera.current;
            if(cam) {
                string outPut = string.Format("{0:0} %", property.health * 100 / property.tb.max_health);
                Vector2 targetPos = cam.WorldToScreenPoint(property.Position + new Vector3(0, 1.0f, 0));
                GUI.Box(new Rect(targetPos.x-30, Screen.height - targetPos.y, 60, 20), outPut);
            }
        }
        protected bool IsMoving {
			get { return agent && (agent.desiredVelocity.magnitude > 0);}
		}
		
		protected void FixedUpdate () {
			if(agent)
				SetAnimationFloat("forward",agent.velocity.magnitude);
			// basic hp regeneration
			if(property.isAlive && property.health < property.tb.max_health)
				property.health += Time.fixedDeltaTime * 0.1f;
		}

		void OnTriggerEnter(Collider col){
			Debug.Log("hit+:" + gameObject.name);

			ObjectProperty uInfo = col.GetComponent<ObjectProperty>();
			if(uInfo) {
				switch(uInfo.type) {
					case ObjectType.Bullet:
						OnDamage(uInfo);
						break;
				}
			}
		}

		virtual protected void OnAnimationTrigger(string arg) {
			//Debug.Log("OnAnimationTrigger:" + arg);
		}

		void SetAnimationTrigger(string arg) {
			Debug.Assert(null != animator,"animator is null " + this.GetType().Name);
			if(animator)
				animator.SetTrigger(arg);
		}

		void SetAnimationFloat(string arg,float val) {
			Debug.Assert(null != animator,"animator is null " + this.GetType().Name);
			if(animator)
				animator.SetFloat(arg,val);
		}

		float DamageCalculate(ObjectProperty attacker) {
			return attacker.tb.attack_power;
		}

		override public void OnAttack(ObjectProperty target) {
			Debug.Assert(null!=target,"target is null");
			if(target.isAlive) {
				SetAnimationTrigger("fire");
				// weapon
				Debug.Assert(null != weapon,"weapon is null!!");
				if(weapon.Shoot(bullet, property, weapon.transform)) {
					//Debug.Log("Shoot!!");
				}
			}
		}

		override public void OnDamage(ObjectProperty skill) {

			if(!IsAlive)
				return;

			Debug.Assert(null != skill,"skill is null");

			// motion adjust
			if(!agent.isStopped)
				agent.velocity = (agent.velocity.magnitude / 2f) * agent.velocity.normalized;

			// demage calculate
			float impactDamage = DamageCalculate(skill);
			property.health -= impactDamage;
			// call die 
			if(property.health <= 0)
				OnDie(skill);
			else 
				SetAnimationTrigger("damaged");
		}

		override public void OnDie(ObjectProperty skill) {
			Debug.Assert(IsAlive,"die again,this unit is already death");
			Debug.Log("Die from " + ((null != skill) ? skill.transform.gameObject.name : ""));

            property.health = 0;
			property.isAlive = false;
			SetAnimationTrigger("dying");
			SpawnManager.instance.Remove(this.property);
		}

		
	}
}