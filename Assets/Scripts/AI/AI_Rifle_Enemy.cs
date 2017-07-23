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
		AIEntity _aiEntity;

		// definition state
		enum eState {
			Wander,
			Search,
			Chase,
			Runaway,
			Attack,
		}

		class AIEntity : Entity {
			public AI_Rifle_Enemy owner;
			public ObjectProperty target;
			
			class FSM : StateTransitionTable {}
			public AIEntity(AI_Rifle_Enemy owner) {
				this.owner = owner;
				transitionTable = new FSM();
				transitionTable.SetState(eState.Wander,new WanderState());
				transitionTable.SetState(eState.Search,new StateSearch());
				transitionTable.SetState(eState.Chase,new StateChase());
				transitionTable.SetState(eState.Runaway,new StateRunaway());
				transitionTable.SetState(eState.Attack,new StateAttack());
			}

		}

		class WanderState : IState {

			Vector3 dest = Vector3.zero;
			public void Enter(Entity e) {
				AIEntity data = (AIEntity)e;
				data.target = null;
			}
			public void Exit(Entity e){}
			public void Execute(Entity e){
				AIEntity data = (AIEntity)e;
				if(data.target == null) {
					if(Facade_AI.DetectTarget(data.owner.property, out data.target)) {
						data.Event = eState.Chase;
					}
					else if(data.owner.IsStoped) {

						if(Facade_NavMesh.RandomRangePoint(
							data.owner.transform.position, 
							data.owner.property.wander_min_range,
							data.owner.property.wander_max_range, out dest)) {
							data.owner.agent.destination = dest;
						}
					}
				}

				Debug.DrawRay(data.owner.transform.position, dest - data.owner.transform.position, Color.green);

			}
		}
		class StateSearch : IState {

			public void Enter(Entity e) {}
			public void Exit(Entity e){}
			public void Execute(Entity e){
				AIEntity data = (AIEntity)e;

				data.Event = eState.Wander;
			}
		}
		class StateChase : IState {
			public void Enter(Entity e) {
				AIEntity data = (AIEntity)e;
				if(data.target != null) {
					data.owner.agent.destination = data.target.transform.position;
				}
			}
			public void Exit(Entity e){}
			public void Execute(Entity e){
				AIEntity data = (AIEntity)e;

				// check reach them and something
				if(data.owner.IsStoped)
					data.Event = eState.Search;
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

		AI_Rifle_Enemy() {
			_aiEntity = new AIEntity(this);
			_aiEntity.Event = eState.Wander;
		}

		// Use this for initialization
		protected void Awake () {
			base.Awake();
		}
		
		protected void OnEnable()
		{
			base.OnEnable();
			//Debug.Log("Enabled 1/"+ this.GetType().Name);
		}
		// Update is called once per frame
		void Update () {

			if(IsAlive)
				_aiEntity.UpdateState();
		}

		override public void OnDamage(ObjectProperty uInfo) {
			base.OnDamage(uInfo);
		}		
		
	}
}