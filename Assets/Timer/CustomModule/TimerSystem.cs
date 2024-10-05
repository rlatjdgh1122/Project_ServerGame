using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Seongho.TimerSystem
{
	public static class TimerSystem
	{
		private static GameObject _timerObj = null;
		private static TimerExecutor _timerExecutor = null;
		private static Dictionary<string, ITimer> _nameToTimerDic = null;

		static TimerSystem()
		{
			_nameToTimerDic = new Dictionary<string, ITimer>();

			CreateExecutor();
		}

        /// <summary>
        /// MonoBehaviour와 GameObject의 기능을 사용하기 위해 TimerExecutor 형태의 오브젝트를 하나 생성해준다.
        /// </summary>
        private static void CreateExecutor()
		{
			if (_timerObj != null)
			{
				Object.Destroy(_timerObj);
			}

			_timerObj = new GameObject("TimerExecutor");
			Object.DontDestroyOnLoad(_timerObj);
			_timerExecutor = _timerObj.AddComponent<TimerExecutor>();
		}

		/// <summary>
		/// 타이머를 생성해주는 함수
		/// </summary>
		public static ITimer CreateTimer(string timerName, float timer)
		{
			if (_nameToTimerDic.ContainsKey(timerName))
			{
				Debug_S.LogError($"{timerName}이 중복되었습니다.");

				return null;

			} //end if

			ITimer iTimer = _timerExecutor.CreateTimer(timerName, timer);
			_nameToTimerDic.Add(timerName, iTimer);

			return iTimer;
		}

		/// <summary>
		/// 타이머를 이름으로 가져오는 함수
		/// </summary>
		public static ITimer GetTimer(string timerName)
		{
			if (_nameToTimerDic.TryGetValue(timerName, out ITimer value))
			{
				return value;

			} //end if

			Debug_S.Log($"{timerName}을 찾을 수 없습니다.");

			return null;
		}

		/// <summary>
		/// 타이머를 지워주는 함수
		/// </summary>
		/// <param name="timerName"></param>
		public static void DeleteTimer(string timerName)
		{
			_nameToTimerDic.Remove(timerName);
		}

		#region Control

		/// <summary>
		/// 모든 타이머를 실행해주는 함수
		/// </summary>
		public static void StartAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.StartTimer();

			} // end foreach
		}

		/// <summary>
		/// 모든 타이머를 정지시키는 함수 
		/// </summary>
		public static void StopAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.StopTimer();

			} // end foreach
		}

        /// <summary>
        /// 모든 타이머를 다시 시작하는 함수 
        /// </summary>
        public static void ReStartAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.ReStartTimer();

			} // end foreach
		}

        /// <summary>
        /// 모든 타이머를 리셋하는 함수 
        /// </summary>
        public static void ResetAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.ResetTimer();

			} // end foreach
		}

        /// <summary>
        /// 모든 타이머를 지우는 함수 
        /// </summary>
        public static void DeleteAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.DeleteTimer();

			} // end foreach
		}

		#endregion

		/// <summary>
		/// 이벤트 구독을 대신해주는 유틸 기능 함수
		/// </summary>
		public static void OnResterTimerEvent(ITimer timer, ITimerEventHandler timerEventHandler)
		{
			timer.OnStartTimerEvent    += timerEventHandler.OnStartTimerEvent;
			timer.OnRunningTimerEvent  += timerEventHandler.OnRunningTimerEvent;
			timer.OnStopTimerEvent     += timerEventHandler.OnStopTimerEvent;
			timer.OnEndTimerEvent      += timerEventHandler.OnEndTimerEvent;
			timer.OnResetTimerEvent    += timerEventHandler.OnResetTimerEvent;
			timer.OnDeleteTimerEvent   += timerEventHandler.OnDeleteTimerEvent;
		}

        /// <summary>
        /// 이벤트 구독해제를 대신해주는 유틸 기능 함수
        /// </summary>
        public static void RemoveResterTimerEvent(ITimer timer, ITimerEventHandler timerEventHandler)
		{
			timer.OnStartTimerEvent    -= timerEventHandler.OnStartTimerEvent;
			timer.OnRunningTimerEvent  -= timerEventHandler.OnRunningTimerEvent;
			timer.OnStopTimerEvent     -= timerEventHandler.OnStopTimerEvent;
			timer.OnEndTimerEvent      -= timerEventHandler.OnEndTimerEvent;
			timer.OnResetTimerEvent    -= timerEventHandler.OnResetTimerEvent;
			timer.OnDeleteTimerEvent   -= timerEventHandler.OnDeleteTimerEvent;
		}


	}

	public class TimerExecutor : MonoBehaviour
	{
		/// <summary>
		/// 여기서 실질적으로 타이머를 생성해준다.
		/// </summary>
		public ITimer CreateTimer(string timerName, float timer)
		{
			Timer iTimer = new GameObject(timerName).AddComponent<Timer>();
			iTimer.SetTimer(timer);

			iTimer.transform.parent = transform;

			return iTimer;
		}
	}
}

