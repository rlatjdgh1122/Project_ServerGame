using System;
using System.Collections;

namespace Seongho.TimerSystem
{
	/// <summary>
	/// 추상화된 타이머. 추후에 새로운 타이머 시스템을 제작할 때를 대비하여 추상화시킴
	/// </summary>
	public interface ITimer
	{
		public event Action OnStartTimerEvent;            //타이머가 시작할때
		public event Action<float> OnRunningTimerEvent;   //타이머가 실행중일때 (루프)
		public event Action OnStopTimerEvent;             //타이머가 멈췄을때
		public event Action OnEndTimerEvent;              //타이머가 끝났을때
		public event Action OnResetTimerEvent;            //타이머가 리셋됐을때
		public event Action OnDeleteTimerEvent;           //타이머가 지워졌을때
		public TimerState TimerState { get; }

		public void StartTimer();
		public void StopTimer();
		public void ReStartTimer();
		public void ResetTimer();
		public void DeleteTimer();

		public void AddTime(float time);
		public void RemoveTime(float time);

		public float GetCurrentTime();
		public float GetMaxTime();
	}
}