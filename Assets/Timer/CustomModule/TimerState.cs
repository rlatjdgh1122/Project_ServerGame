namespace Seongho.TimerSystem
{
	public enum TimerState : sbyte
	{
		None = -1,
		Start,
		Running,
		Stopped,
		Ended,
		Reset,
		Delete,
	}
}