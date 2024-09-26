namespace Seongho.TimerSystem
{
	public interface ITimerEventHandler
	{
		public void OnStartTimerEvent();
		public void OnRunningTimerEvent(float curTimer);
		public void OnStopTimerEvent();
		public void OnEndTimerEvent();
		public void OnDeleteTimerEvent();
	}
}