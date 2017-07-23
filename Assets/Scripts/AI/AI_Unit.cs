﻿using System.Collections;
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
			
			//Debug.Log("Enabled 0/"+ this.GetType().Name);
							
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
					agent.isStopped = true;
					break;
				case "leaveDamage":
					agent.isStopped = false;
					break;
				case "enterDie":
					agent.isStopped = true;
					break;
				case "leaveDie":
					DisposeForPool();
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

		float DamageCalculate(ObjectProperty attacker) {
			return attacker.attack_power;
		}

		override public void OnDamage(ObjectProperty attacker) {

			if(!IsAlive)
				return;

			Debug.Assert(null != attacker,"Attacker is null");

			// motion adjust
			if(!agent.isStopped)
				agent.velocity = (agent.velocity.magnitude / 2f) * agent.velocity.normalized;

			// demage calculate
			float impactDamage = DamageCalculate(attacker);
			property.life -= impactDamage;
			// call die 
			if(property.life <= 0) {
				OnDie(attacker);
			}
			else 
				SetAnimationTrigger("damaged");
		}
		override public void OnDie(ObjectProperty attacker) {
			Debug.Assert(IsAlive,"die again,this unit is already death");
			Debug.Log("Die from " + ((null != attacker)?attacker.transform.gameObject.name : ""));
			
			SetAnimationTrigger("dying");
			SpawnManager.instance.Remove(this.property);
		}

		
	}
}