using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SB {
	public enum ERROR_CODE
	{
		NONE					= 0,
		SUCCESS 				= 1,
		FAIL					= -1,
		CANCEL					= -2,
		
		DUPLICATE_NAME			= -1001,	// 이미 있는 이름이다.
		INVALID_LENGTH			= -1002,	// 문자열 길이가 잘못 되었다.
		SAME_NICKNAME			= -1003,	// 동일한 닉네임이다.
		
		NOT_ENOUGH_CANDY		= 1100,
		NOT_ENOUGH_DIA			= 1101,
		NOT_ENOUGH_GOLD			= 1102,
		NOT_ENOUGH_GIFTPOINT	= 1103,
		NOT_ENOUGH_TICKET		= 1104,
		NOT_ENOUGH_MILEAGEDIA	= 1105,			// 다이아 연성 포인트 부족
		NOT_ALLOWED_MILEAGEDIA	= 1106,			// 다이아 연성은 하루에 한번만		
	}	



	// 코루틴이 끝났을때 처리를 콜백으로 처리 하지 말자.
	public class ActionContext
	{
		public ERROR_CODE _error_code = ERROR_CODE.NONE;	// 에러 코드
		public IEnumerator _runContexts = null;				// 시퀀스 전달
		public TaskRun _jobTaskRun = null;
		public Action<ERROR_CODE> _callbackException;		// 예외 처리가 필요하면 오출해..

		public bool isDone { get; set; }	// 완료상태 구분..
		
		// 생성자.
		public ActionContext(IEnumerator run = null)
		{
			_runContexts = run;
		}

		// 클리어
		public void Clear()
		{
			_error_code = ERROR_CODE.NONE;	// 에러 코드
			_jobTaskRun = null;
			_runContexts = null;
		}

		// 복사해와
		public void Copy(ActionContext info)
		{
			_error_code = info._error_code;	// 에러 코드
			_jobTaskRun = info._jobTaskRun;
			_runContexts = info._runContexts;
			_callbackException = info._callbackException;
			isDone = info.isDone;
		}

		// 끝났어요
		public void Done(ERROR_CODE error_code)
		{
			_error_code = error_code;
			Debug.LogFormat("[End] Task: {0} / {1}", _jobTaskRun.curr_task.state, error_code);
			// 동작중 이상 상태로 실패하면 기록 해야지
			//workTaskRun.nowTask.err_code = error_code; // Done 처리 할지 말지 결정 하는부분
		}
	}
}