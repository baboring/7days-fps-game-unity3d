using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SB {

	[RequireComponent(typeof(NavMeshAgent))]
	public class AI_Unit : Unit {

		protected NavMeshAgent agent;
		private Animator animator;
		Vector3 moveDirection = Vector3.zero;

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
			agent.speed = property.walkSpeed;
			agent.angularSpeed = property.angularSpeed;
			agent.stoppingDistance = property.stoppingDist;
			
			Debug.Log("Enabled 0/"+ this.GetType().Name);
							
		}
		protected bool IsStoped {
			get { return agent && (agent.isStopped || !agent.hasPath);}
		}
		
		void FixedUpdate () {
			if(agent)
				SetAnimationFloat("forward",agent.velocity.magnitude);
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

		public void OnAnimationTrigger(string arg) {
			Debug.Log("OnAnimationTrigger:" + arg);
			switch(arg) {
				case "enterDamage":
					agent.Stop();
					break;
				case "leaveDamage":
					agent.Resume();
					break;
				case "enterDie":
					agent.Stop();
					break;
				case "leaveDie":
					this.property.ReturnToPool();
					break;
			}
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

		override public void OnDamage(ObjectProperty uInfo) {

			if(!IsAlive)
				return;

			Debug.Assert(null != uInfo,"Attacker is null");

			agent.velocity = (agent.velocity.magnitude / 2f) * agent.velocity.normalized;
			property.life -= uInfo.attack_power;

			if(IsAlive)
				SetAnimationTrigger("damaged");
			else
				SetAnimationTrigger("dying");
		}
		
	}
}