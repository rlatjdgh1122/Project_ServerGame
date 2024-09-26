using System;
using System.Collections;

namespace Seongho.TimerSystem
{
	public interface ITimer
	{
		public event Action OnStartTimerEvent;            //Ÿ�̸Ӱ� �����Ҷ�
		public event Action<float> OnRunningTimerEvent;   //Ÿ�̸Ӱ� �������϶� (����)
		public event Action OnStopTimerEvent;             //Ÿ�̸Ӱ� ��������
		public event Action OnEndTimerEvent;              //Ÿ�̸Ӱ� ��������
		public event Action OnDeleteTimerEvent;           //Ÿ�̸Ӱ� ����������
		public TimerState TimerState { get; }

		public void StartTimer();
		public void StopTimer();
		public void ReStartTimer();
		public void ResetTimer();
		public void DeleteTimer();

		public void AddTime(float time);
		public void RemoveTime(float time);

		public float GetCurrentTime();
	}
}