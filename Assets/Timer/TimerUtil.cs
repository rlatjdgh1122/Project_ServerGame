using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Seongho.TimerSystem
{
	public static class TimerUtil
	{
		private static GameObject _timerObj = null;
		private static TimerExecutor _timerExecutor = null;
		private static Dictionary<string, ITimer> _nameToTimerDic = null;

		static TimerUtil()
		{
			_nameToTimerDic = new Dictionary<string, ITimer>();
			CreateExecutor();
		}

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

		public static ITimer GetTimer(string timerName)
		{
			if (_nameToTimerDic.TryGetValue(timerName, out ITimer value))
			{
				return value;

			} //end if

			Debug_S.Log($"{timerName}을 찾을 수 없습니다.");

			return null;
		}

		public static void DeleteTimer(string timerName)
		{
			_nameToTimerDic.Remove(timerName);
		}

		#region Control

		public static void StartAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.StartTimer();

			} // end foreach
		}

		public static void StopAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.StopTimer();

			} // end foreach
		}

		public static void ReStartAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.ReStartTimer();

			} // end foreach
		}

		public static void ResetAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.ResetTimer();

			} // end foreach
		}

		public static void DeleteAllTimer()
		{
			foreach (ITimer item in _nameToTimerDic.Values)
			{
				item.DeleteTimer();

			} // end foreach
		}

		#endregion

		/// <summary>
		/// 이벤트 연결을 대신해주는 유틸 기능 함수
		/// </summary>
		public static void OnResterTimerEvent(ITimer timer, ITimerEventHandler timerEventHandler)
		{
			timer.OnStartTimerEvent    += timerEventHandler.OnStartTimerEvent;
			timer.OnRunningTimerEvent  += timerEventHandler.OnRunningTimerEvent;
			timer.OnStopTimerEvent     += timerEventHandler.OnStopTimerEvent;
			timer.OnEndTimerEvent      += timerEventHandler.OnEndTimerEvent;
			timer.OnDeleteTimerEvent   += timerEventHandler.OnDeleteTimerEvent;
		}

		public static void RemoveResterTimerEvent(ITimer timer, ITimerEventHandler timerEventHandler)
		{
			timer.OnStartTimerEvent    -= timerEventHandler.OnStartTimerEvent;
			timer.OnRunningTimerEvent  -= timerEventHandler.OnRunningTimerEvent;
			timer.OnStopTimerEvent     -= timerEventHandler.OnStopTimerEvent;
			timer.OnEndTimerEvent      -= timerEventHandler.OnEndTimerEvent;
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

