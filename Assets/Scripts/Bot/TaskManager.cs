/********************************************************************
	created:	2014/12/09
	filename:	TaskManager.cs
	author:		Benjamin
	purpose:	[]
*********************************************************************/
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SB
{

	public class TaskManager
	{
		//public TaskRun Current = null;
		TaskRun Current = new TaskRun(new DesireKey());
		// 정해진대로 욕구 처리 해~
		List<DesireKey> lstSequence = null;

		private BVT_FSM fsmTask = new BVT_FSM();	// 메뉴 이동 봇 시퀀스

		// 욕구를 이어가는 카테고리식..
		Stack<TaskRun> stacks = new Stack<TaskRun>();

		// 순서대로 해야 하는거야?
		bool hasSequence { get { return (null != lstSequence); } }

		// 작업 완료 했어요
		public bool IsDone = false;

		// 초기화는 여기서.
		public TaskManager( BVT_FSM _fsmTask )
		{
			fsmTask = _fsmTask;
		}

		public void SetManualOrder(List<DesireKey> manual_list)
		{
			lstSequence = manual_list;
		}


		// 뭐 하고 싶어요? 추가
		void PushDesire(DesireKey desireKey)
		{
			//stacks.Push(BotBuilder.BuildDesire(desireKey));
		}

		// 해야 할 욕구덩어리를 가져와 봐!
		TaskRun PopDesire()
		{
			// 다음 해야할 것들 
			if (stacks.Count > 0)
				return stacks.Pop();

			return null;
		}

		// 수행 하자구
		public IEnumerator Perform()
		{
			yield return null;	// null exception 방지용
			// FSM은 꼭 있어야지...
			Assert.IsNotNull(fsmTask, "fsmTask is null");
			if (null == fsmTask)
				yield break;

			// 여기서 다시 넣는건 테스트 용이다... 그냥 Current 처리 해도 되는데 넣었다가 빼자..
			if (stacks.Count < 1) {
				DesireKey new_desire;
				// 순서 대로 해야 한다면...
				if (hasSequence && lstSequence.Count > 0) {

					new_desire = lstSequence[0];
					lstSequence.RemoveAt(0);
				}
				else
					new_desire = new DesireKey();

				PushDesire(new_desire);
			}
			// 하고싶은걸 해야지
			Current = PopDesire();

			Assert.IsNotNull(Current, "current desire is null");
			if (null == Current) {
				this.IsDone = true;
				yield break;
			}

			// 자 작업 시작..
			Current.BeginPerform();

			// PerformTask 대기
			var jobTasks = Current.DoPerform(fsmTask).GetEnumerator();
			while (jobTasks.MoveNext())
				yield return jobTasks.Current;

			// 여기 올때는 무조건 task는 End of Task 이다.
			// 작업수행후 결과에 대한 처리 해야지
			Current.EndPerform();


			// 나갈꺼란다..
			if (Current.state == DesireState.Quit)
				this.IsDone = true;

			// 뭔가 이유로 인해서 완료 못했나 보다... 선행 할것이 있다면 넣어주자.
			if (Current.preDesire.state != DesireState.None) {

				bool isTryable = false;
				// 이미 하고자 하는게 있으면 중복해서 넣으면 무한 루프에 빠질 수 있다.
				//if (!Exists(Current) && !Exists(new Desire(Current.preDesire)))
					isTryable = true;

				// 다시 넣을 수 있다.
				if (isTryable) {
					// 다시 넣자..
					Debug.Log("Push last desire : " + Current.state);
					PushDesire(Current);
					// 해결할 놈을 넣자...
					Debug.Log("Push pre desire :" + Current.preDesire.state);
					PushDesire(Current.preDesire);
				}
				else {
					Debug.Log("isTryable fail" + Current.state);
				}
			}
			Current = null;

		}

		// 이미 존재 하는지 검사해
		//bool Exists(Desire _state)
		//{
		//    return Array.Exists(stacks.ToArray(), va => va.IsEqual(_state));

		//}

	}

}