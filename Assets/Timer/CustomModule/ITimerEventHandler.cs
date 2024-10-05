namespace Seongho.TimerSystem
{
    /// <summary>
    /// 타이머의 이벤트를 실행해주고 싶을 경우 상속받아 사용한다.
    /// </summary>
	public interface ITimerEventHandler
	{
		public void OnStartTimerEvent() { }
		public void OnRunningTimerEvent(float curTimer) { }
        public void OnStopTimerEvent() { }
        public void OnEndTimerEvent() { }
        public void OnResetTimerEvent() { }
        public void OnDeleteTimerEvent() { }
    }
}