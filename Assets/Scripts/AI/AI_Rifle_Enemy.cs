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
		AIEntity _aiEntity = new AIEntity();

		// definition state
		enum eState {
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
				transitionTable.SetState(eState.Wander,new WanderState());
				transitionTable.SetState(eState.Search,new StateSearch());
				transitionTable.SetState(eState.Chase,new StateChase());
				transitionTable.SetState(eState.Runaway,new StateRunaway());
				transitionTable.SetState(eState.Attack,new StateAttack());
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
					if(Facade_AI.DetectTarget(entity.property, out entity.target)) {
						Debug.Log("- Detect palyer!!");
						// check distance for attacking or  
						Vector3 distance = entity.target.transform.position - entity.property.transform.position;
						if(distance.magnitude < entity.property.info.attackRange)
							entity.Event = eState.Attack;
						else
							entity.Event = eState.Chase;
						return;
					}
					// check point 2
					else if(entity.Ai.IsStoped) {

						if(Facade_NavMesh.RandomRangePoint(
							entity.property.transform.position, 
							entity.property.info.wander_min_range,
							entity.property.info.wander_max_range, out entity.destPos)) {
							entity.Ai.agent.destination = entity.destPos;
						}
					}
				}
			}
		}
		class StateSearch : IState {

			public void Enter(Entity e) {}
			public void Exit(Entity e){}
			public void Execute(Entity e){
				AIEntity entity = (AIEntity)e;

				entity.Event = eState.Wander;
			}
		}

		// Chasing target
		class StateChase : IState {
			public void Enter(Entity e) {
				AIEntity entity = (AIEntity)e;
				entity.Ai.agent.speed = entity.property.info.runSpeed;
				if(entity.target != null) {
					entity.Ai.agent.destination = entity.target.transform.position;
				}
			}
			public void Exit(Entity e){
				AIEntity entity = (AIEntity)e;
				entity.Ai.agent.speed = entity.property.info.walkSpeed;
			}
			public void Execute(Entity e){
				AIEntity entity = (AIEntity)e;

				// check reach them and something
				if(entity.Ai.IsStoped)
					entity.Event = eState.Search;
			}
		}

		class StateAttack : IState {
			public void Enter(Entity e) {}
			public void Exit(Entity e){}
			public void Execute(Entity e){

			}
		}	
		class StateRunaway : IState {
			public void Enter(Entity e) {}
			public void Exit(Entity e){}
			public void Execute(Entity e){

			}
		}

		// Use this for initialization
		protected void Awake () {
			base.Awake();
		}
		
		protected void OnEnable()
		{
			base.OnEnable();
			_aiEntity.Reset(this);
			_aiEntity.Event = eState.Wander;
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
		
	}
}