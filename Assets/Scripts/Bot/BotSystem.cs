using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SB {
	public class BVT_FSM : FiniteStateMachine<TaskId> { };

	public class BotSystem : SingletonMB<BotSystem> {


		private BVT_FSM fsmTask = new BVT_FSM();		// ai fsm

		private ActionContext jobContext = new ActionContext();

		static public bool IsEnable { get; private set; }	// 봇 On / Off
		public bool IsRunning { get; private set; }	// 봇 동작 상태

		void Awake() {
			fsmTask.Initialize(TaskId.None);
		}
		// Use this for initialization
		void Start () {
			DoCoroutine(_MainBotDesireSystem(null));
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		// 봇 상태를 클리어 하자.. 가능하나 ㅜㅜ
		public void Reset()
		{
			fsmTask.Initialize(TaskId.None);
		}

		// 너무 길어 좀 줄이자.
		void Register(TaskId init, TaskId end, IEnumerable _coroutine, Callback LeaveCall = null, Callback enterCall = null)
		{
			fsmTask.AddTransition(init, end, null, LeaveCall, enterCall, _coroutine);
		}		
//////////////////////////////////////////////////////////////////////////
		/// AI형 패턴 봇........
		IEnumerator _MainBotDesireSystem(List<DesireKey> manual_order)
		{
			IsRunning = true;

			// 봇 상태 꺼질때까지 무한 반복한다.
			TaskManager taskManager = new TaskManager(fsmTask);
			taskManager.SetManualOrder(manual_order);

			//////////////////////////////////////////////////////////////////////////
			while (IsEnable && !taskManager.IsDone) // 여러번 반복이면 하자.
			{
				// 포즈 누르면 끝~~
 				if (Input.GetKeyDown(KeyCode.Pause))
 					IsEnable = false;
				yield return DoCoroutine(new ActionContext(taskManager.Perform()));
			}

			// 끝났다.
			IsRunning = false;
			Debug.Log("Stop BotSystem !!!");

		}

		// 코루틴 끝날때까지 대기하고 끝...
		IEnumerator _WaitEndCoroutine(ActionContext _info)
		{
			//Debug.Log(ColorType.magenta, "wait Coroutine !! ");

			// 어딘가에서 아직 사용하고 있군...
			Assert.IsTrue(jobContext._error_code == ERROR_CODE.NONE, "error !! " + jobContext._error_code);

			//////////////////////////////////////////////////////////////////////////
			// 보관 해야지
			jobContext.Copy(_info);

			yield return DoCoroutine(_info._runContexts);

			// 값을 복사해서 가자..
			_info.Copy(jobContext);

			//Debug.Log(ColorType.yellow, "task result : " + _info._error_code);

			// 결과전달
			var workTaskRun = _info._jobTaskRun;
			if (null != workTaskRun) {
				workTaskRun.curr_task.err_code = _info._error_code;

				if (!workTaskRun.curr_task.isDone)
					workTaskRun.curr_task.err_code = ERROR_CODE.SUCCESS;
			}

			// 끝났다.
			_info.isDone = true;

			// 클리어 하자.
			jobContext.Clear();
		}

		// 코루틴 시작해요
		public static Coroutine DoCoroutine(ActionContext inst_info)
		{
			Assert.IsNotNull(inst_info._runContexts, "instruction is null");
			//Assert.IsFalse(!inst_info._workTaskRun.task.isDone, "exist working coroutine !!");
			Debug.LogFormat(">> DoCoroutine => {0} -----------------------", inst_info._runContexts);

			return instance.DoCoroutine(instance._WaitEndCoroutine(inst_info));
		}

		// 코루틴 시작해요
		Coroutine DoCoroutine(IEnumerator _coroutine)
		{
			var co = StartCoroutine(_coroutine);
			Assert.IsNotNull(co, "coroutine is null : " + _coroutine);
			return co;
		}

		// 코루틴 단계별 처리용
		Coroutine DoCoroutine(IEnumerable _coroutine)
		{
			var co = StartCoroutine(_coroutine.GetEnumerator());
			Assert.IsNotNull(co, "coroutine is null : " + _coroutine);
			return co;
		}

		//////////////////////////////////////////////////////////////////////////
		// 테스크 완료

		// 코루틴으로 도는 Complete
		IEnumerable TaskDone(ActionContext jobInst)
		{
			yield return null;
			Debug.Log("TaskDone : " + fsmTask.GetState());

			jobInst.Done(ERROR_CODE.SUCCESS);
		}
		


		// 회원 가입 seq
		void RegisterFSM_Join()
		{
			// Step 1
			Register(TaskId.None, TaskId.Join, TaskJoinAccount(jobContext));
			Register(TaskId.Logout, TaskId.Join, TaskJoinAccount(jobContext));
			Register(TaskId.Join, TaskId.Login, TaskLogin(jobContext));
		}

		//////////////////////////////////////////////////////////////////////////
		// 로그인 테스크 진행
		#region Join proc
		IEnumerable TaskJoinAccount(ActionContext context) {		// _회원가입처리
			yield return new WaitForSeconds(1);

			// 봇 이름을 생성 하자 ( 네트워크 봇은 서버가 준다 )
			// 작업 끝
			context.Done(ERROR_CODE.SUCCESS);
		}
		#endregion		

		#region Login proc
		IEnumerable TaskLogin(ActionContext jobInst) {		// _회원가입처리
			yield return new WaitForSeconds(1);

			// 봇 이름을 생성 하자 ( 네트워크 봇은 서버가 준다 )
			// 작업 끝
			jobInst.Done(ERROR_CODE.SUCCESS);
		}							
		#endregion		
	}
}