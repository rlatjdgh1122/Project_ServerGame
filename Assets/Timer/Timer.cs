using System;
using System.Collections;
using UnityEngine;

namespace Seongho.TimerSystem
{
	public class Timer : MonoBehaviour, ITimer
	{
		public event Action OnStartTimerEvent = null;
		public event Action<float> OnRunningTimerEvent = null;
		public event Action OnStopTimerEvent = null;
		public event Action OnEndTimerEvent = null;
		public event Action OnDeleteTimerEvent = null;

		private TimerState _timerState = TimerState.None;
		public TimerState TimerState => _timerState;

		private float _curTime = 0f;
		private float _stopTime = 0f;
		private float _maxTime = 0f;
		private Coroutine _timerCoroutine = null;

		#region Action

		public void StartTimer()
		{
			_timerState = TimerState.Running;
			OnStartTimerEvent?.Invoke();

			if (_timerCoroutine != null)
			{
				StopCoroutine(_timerCoroutine);
			}

			_timerCoroutine = StartCoroutine(Corou_Timer(_curTime));
		}

		public void StopTimer()
		{
			_stopTime = _curTime;

			_timerState = TimerState.Stopped;
			OnStopTimerEvent?.Invoke();

			if (_timerCoroutine != null)
			{
				StopCoroutine(_timerCoroutine);
				_timerCoroutine = null;

			} //end if

		}

		public void ReStartTimer()
		{
			StopTimer();
			_curTime = _stopTime;
			StartTimer();
		}

		public void ResetTimer()
		{
			_curTime = _maxTime;
		}

		public void DeleteTimer()
		{
			OnDeleteTimerEvent?.Invoke();
			TimerUtil.DeleteTimer(name);
			Destroy(gameObject);
		}

		#endregion

		#region Value

		public void AddTime(float time)
		{
			_curTime += time;
		}

		public void RemoveTime(float time)
		{
			_curTime -= time;
		}

		#endregion

		public float GetCurrentTime()
		{
			return _curTime;
		}

		public void SetTimer(float timer)
		{
			_maxTime = _curTime = timer;
		}

		private IEnumerator Corou_Timer(float timer)
		{
			_curTime = timer;

			while (_curTime > 0f)
			{
				_curTime -= Time.deltaTime;
				OnRunningTimerEvent?.Invoke(_curTime);
				yield return null;
			}

			_curTime = 0f;
			OnRunningTimerEvent?.Invoke(0f); //확실하게 0초로 지정

			_timerState = TimerState.Ended;
			OnEndTimerEvent?.Invoke();
		}
	}
}
