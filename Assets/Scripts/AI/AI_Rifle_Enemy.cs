/* *************************************************
*  Created:  7/20/2017, 2:13:40 PM
*  File:     AI_Rifle_Enemy.cs
*  Author:   Benjamin
*  Purpose:  []
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Ai;
using SB;

namespace SB {


	public class AI_Rifle_Enemy : AI_Unit {

		// Use this for initialization
		protected void Awake () {
			base.Awake();
		}
		
		protected void OnEnable()
		{
			base.OnEnable();
			_aiEntity.Reset(this);
			_aiEntity.Event = eEntityState.Wander;
			//Debug.Log("Enabled 1/"+ this.GetType().Name);
		}
		// Update is called once per frame
		void Update () {

			if(IsAlive && _aiEntity.Ai) {
				_aiEntity.UpdateState();
				if(_aiEntity.destPos != Vector3.zero)
					Debug.DrawRay(_aiEntity.property.transform.position, _aiEntity.destPos - _aiEntity.property.transform.position, Color.green);
				
			}
		}

		override public void OnDamage(ObjectProperty uInfo) {
			base.OnDamage(uInfo);
		}		
		
				
		AIEntity _aiEntity = new AIEntity();

		// definition state
		enum eEntityState {
			Wander,
			Search,
			Chase,
			Runaway,
			Attack,
		}

		class AIEntity : Entity {
			public AI_Rifle_Enemy Ai;
			public ObjectProperty property;
			public ObjectProperty target;
			public Vector3 destPos = Vector3.zero;

			class FSM : StateTransitionTable {}
			public AIEntity() {
				transitionTable = new FSM();
				transitionTable.SetState(eEntityState.Wander,new WanderState());
				transitionTable.SetState(eEntityState.Search,new StateSearch());
				transitionTable.SetState(eEntityState.Chase,new StateChase());
				transitionTable.SetState(eEntityState.Runaway,new StateRunaway());
				transitionTable.SetState(eEntityState.Attack,new StateAttack());
			}

			public void Reset(AI_Rifle_Enemy Ai) {
				this.Ai = Ai;
				this.property = Ai.property;
				this.target = null;
				this.destPos = Vector3.zero;
			}

		}

		// Wander around
		class WanderState : IState {

			public void Enter(Entity e) {
				AIEntity entity = (AIEntity)e;
				entity.target = null;
			}
			public void Exit(Entity e){}
			public void Execute(Entity e){
				AIEntity entity = (AIEntity)e;

				// check target
				if(entity.target == null) {
					// check point 1
					if(Facade_AI.DetectTarget(entity.property, entity.property.tb.sightRange, out entity.target)) {
						Debug.Log("- Detect palyer!!");
						// check distance for attacking or  
						Vector3 distance = entity.target.transform.position - entity.property.transform.position;
						if(distance.magnitude < entity.property.tb.attackRange)
							entity.Event = eEntityState.Attack;
						else
							entity.Event = eEntityState.Chase;
						return;
					}
					// check point 2
					else if(!entity.Ai.IsMoving) {

						if(Facade_NavMesh.RandomRangePoint(
							entity.property.transform.position, 
							entity.property.tb.wander_min_range,
							entity.property.tb.wander_max_range, out entity.destPos)) {
							entity.Ai.agent.destination = entity.destPos;
						}
					}
				}
			}
		}

		// Search target
		#region State Search ---------------
		class StateSearch : IState {

			public void Enter(Entity e) {}
			public void Exit(Entity e){}
			public void Execute(Entity e){
				AIEntity entity = (AIEntity)e;

				entity.Event = eEntityState.Wander;
			}
		}
		#endregion

		// Chasing target
		#region State Chase ---------------
		class StateChase : IState {
			public void Enter(Entity e) {
				AIEntity entity = (AIEntity)e;
				entity.Ai.agent.speed = entity.property.tb.runSpeed;
				if(entity.target != null) {
					entity.Ai.agent.destination = entity.target.transform.position;
				}
			}
			public void Exit(Entity e){
				AIEntity entity = (AIEntity)e;
				entity.Ai.agent.speed = entity.property.tb.walkSpeed;
			}
			public void Execute(Entity e){
				AIEntity entity = (AIEntity)e;

				// check point 1
				{
					// check attack condition
					ObjectProperty target;					
					if(Facade_AI.DetectTarget(entity.property, entity.property.tb.attackRange, out target)) {
						// replace target and mode change to attack
						entity.Ai.agent.isStopped = true;
						entity.target = target;
						entity.Event = eEntityState.Attack;
						return;
					}
				}
				//check point 2
				{
					// follow target if it is in chaseRange 
					if(entity.target) {
						Vector3 distance = Facade_AI.GetDistance(entity.target, entity.property);
						if(distance.magnitude < entity.property.tb.chaseRange) {
							entity.Ai.agent.destination = entity.target.transform.position;
							return;
						}
					}
				}

				// check reach them and something
				if(!entity.Ai.IsMoving)
					entity.Event = eEntityState.Wander;
			}
		}
		#endregion


		// Attack State 
		#region State Attack ---------------
		class StateAttack : IState {
			private float elapsedTime; 
			public void Enter(Entity e) {
				AIEntity entity = (AIEntity)e;
				entity.Ai.agent.isStopped = true;
				entity.Ai.OnAttack(entity.target);
				elapsedTime = float.MaxValue;
			}
			public void Exit(Entity e){
				AIEntity entity = (AIEntity)e;
				if(entity.Ai.animator)
					entity.Ai.animator.SetBool("attack",false);
			}
			public void Execute(Entity e){
				AIEntity entity = (AIEntity)e;

				// check point 1
				{
					// check still in attack range ( not in sight)
					if(!Facade_AI.IsRaycastHit(entity.property, entity.target, entity.property.tb.attackRange)) {
						// replace target and mode change to attack
						Vector3 distance = Facade_AI.GetDistance(entity.target, entity.property);
						if(distance.magnitude < entity.property.tb.chaseRange)
							entity.Event = eEntityState.Chase;
						else
							entity.Event = eEntityState.Search;
						return;
					}
				}
				// check point 2
				{
					float temp_interval_for_attack = 1.0f;
					// attack processing
					if(elapsedTime > temp_interval_for_attack) {
						elapsedTime = 0;
						// attack
						entity.Ai.OnAttack(entity.target);
					}
					if(entity.Ai.animator)
						entity.Ai.animator.SetBool("attack",true);
					// increase elapse time
					elapsedTime += Time.deltaTime;
				}

				float slerpTime = 10.0f;
				// look at the target
				Vector3 look = Facade_AI.GetDistance(entity.target, entity.property).normalized;
				entity.property.transform.forward = Vector3.Lerp(entity.property.transform.forward, look, slerpTime * Time.deltaTime);


			}
		}	
		#endregion


		// Attack Runaway 
		#region State Attack ---------------
		class StateRunaway : IState {
			public void Enter(Entity e) {}
			public void Exit(Entity e){}
			public void Execute(Entity e){

			}
		}
		#endregion
		


	}
}