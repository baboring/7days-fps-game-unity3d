using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {
	
	public enum TaskId {
		None,
		Logout,
		Login,
		Join,
		EndOfTask
	}	
	
	// status to do take for AI
	public class TaskStatus {
		public TaskId state;
		public ERROR_CODE err_code = ERROR_CODE.NONE;	// 에러 코드
		public bool isDone { get { return err_code != ERROR_CODE.NONE; } }

		public TaskStatus(TaskId state) {
			this.state = state;
		}
	}

	// 봇이 하고자 하는 단계별 목표점
	public enum DesireState
	{
		None = 0,
		Buy,				// 아이템 사야지
		BuyBag,				// 가방 사야지
		ArenaGame,			// 아레나 가서 겜해야지
		SingleGame,			// 싱글 게임 해야지
		Quit,				// 봇 종료 해야지
		Reboot,				// 리부팅 해야지
		Max
	}	

	//구체적인 욕구
	public enum eDesireItem
	{
		None = 0,
		Dia,				// 다이아
		Candy,				// 사탕
		Refresh,			// 새로고침

		Max
	}

	// 키 처리 해야지...
	public class DesireKey {
		public DesireState state;		// 현재 상태

		public class Content
		{
			public eDesireItem detail;		// 상세 항목
			public int idx;			// key Index
			public int count;		// 갯수

			public Content(eDesireItem _detail = eDesireItem.None) {
				detail = _detail;
			}
		}

		public List<Content> contents = new List<Content>();		// 항목들

		// 생성 
		public DesireKey(DesireState _state = DesireState.None)
		{
			state = _state;
		}

		// 비교 override
		//public bool IsEqual(DesireKey op1)
		//{
		//    // 둘다 널이면 같은거임..
		//    if (null == op1)
		//        return false;
		//    //return (op1.KeyID == op2.KeyID );
		//    return (op1.state == this.state && op1.detail == this.detail);
		//}

	}

	// 일단 만들어 보자.
	public class TaskRun : DesireKey
	{

		// 테스크로 진행 하자...
		private List<TaskStatus> listTask = new List<TaskStatus>();
		int try_count;			// 해당 욕구를 못 채웠으면 일정한 수준이 되었을때 포기 해야해...
		ERROR_CODE _err_code = ERROR_CODE.NONE;	// 완료 여부
		public DesireKey preDesire = new DesireKey();	// 충족해야할 욕구가 있다.

		public ERROR_CODE err_code {
			get { return _err_code; }
			set { _err_code = value; }
		}

		// constant for comparing 
		static TaskStatus endOfTask = new TaskStatus(TaskId.EndOfTask) { err_code = ERROR_CODE.SUCCESS };

		// 현재 진행중인 테스크
		public TaskStatus curr_task {
			get {
				if (this.listTask.Count < 1)
					return endOfTask;
				return this.listTask[0];
			}
		}

		// constructor 
		public TaskRun(DesireKey deKey) {
			state = deKey.state;
			contents = deKey.contents;	// 하던거 있으면 이것도 받아요...
		}

		// 수행 카운트
		public bool BeginPerform(){		
			++try_count;	// try count for performing limit
			Debug.Log(">>> Perform (Begin) : "+state);
			//BotBuilder.BuildTaskSequnce(this);
			return true;
		}

		// 끝났다...
		public void EndPerform() {
			Debug.LogFormat(">>> Perform (End) :{0} / {1} /  {2}\r\n", state, this.curr_task.state, err_code);

			if (try_count > 5 && _err_code != ERROR_CODE.SUCCESS)	// 켄슬이나 다른 에러가 많으면 실패 처리해야지
				_err_code = ERROR_CODE.FAIL;

			switch (err_code) {
			case ERROR_CODE.SUCCESS:
			case ERROR_CODE.FAIL:
			case ERROR_CODE.CANCEL:
				break;
			case ERROR_CODE.NOT_ENOUGH_CANDY:
				this.SetPreDesire(new DesireKey(DesireState.Buy) { contents = { new Content(eDesireItem.Candy) }});	// 사탕이 부족합니다.
				break;
			default:
				break;
			}

		}

		// 테스크 진행 처리 하자...
		public IEnumerable DoPerform(BVT_FSM fsmTask)
		{
			// 테스크 처리다...
			while (TaskId.EndOfTask != curr_task.state) {

				Debug.Log("[Start] Task : " + curr_task.state);
				if (!fsmTask.Advance(curr_task.state))
					yield break;

				ActionContext context = new ActionContext(fsmTask.routine) { _jobTaskRun = this };
				BotSystem.DoCoroutine(context);

				// 일하는 중이면 끝...
				while (!context.isDone) {
					if(!BotSystem.IsEnable)
						yield break;
					yield return null;
				}

				// 일 끝났으니 지워~
				var lastTask = curr_task;
				bool removed = RemoveTask(lastTask);
				// 내 꺼라면 맞으면 상태에 따라서 처리
				// - 실패 한거면 앞으로 할것들 모두 클리어
				if (lastTask.err_code != ERROR_CODE.SUCCESS || false == removed) {
					Debug.LogFormat(">> Task have an error : {0} , (Clear Task !!)", lastTask.err_code);
					listTask.Clear();

					// 에러 상태를 알아야 한다.
					this.err_code = lastTask.err_code;

				}
				else if (TaskId.EndOfTask == curr_task.state && this.err_code == ERROR_CODE.NONE) {	// 문제 없이 모두 끝마쳤으면 Success
					this.err_code = ERROR_CODE.SUCCESS;
				}

				yield return null;
			}
		}

		// 이게 충족이 안되서 못했어요...
		void SetPreDesire(DesireKey desireKey) {
			preDesire = desireKey;
		}

		// 테스크 추가
		public void AddTask(TaskId task_state) {
			AddTask(new TaskStatus(task_state));
		}

		// 테스크 추가
		public void AddTask(TaskStatus task)  {
			Debug.Log(" > AddTask : " + task.state);
			listTask.Add(task);
		}

		public bool RemoveTask(TaskStatus task)  {
			Debug.Log(" > RemoveTask : " + task.state);
			return listTask.Remove(task);
		}
		
	}


}