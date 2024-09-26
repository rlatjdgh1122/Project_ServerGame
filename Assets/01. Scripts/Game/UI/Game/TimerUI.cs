using Seongho.TimerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUI : ExpansionMonoBehaviour, ITimerEventHandler
{
	ITimer timer = null;

	private void Awake()
	{
		TimerUtil.CreateTimer("테스트", 10);
	}

	private void Start()
	{
		timer = TimerUtil.GetTimer("테스트");
		TimerUtil.OnResterTimerEvent(timer, this);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			timer.StartTimer();
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			timer.StopTimer();
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			timer.ReStartTimer();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			timer.RemoveTime(10);
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			timer.ResetTimer();
		}
	}

	public void OnDeleteTimerEvent()
	{

	}

	public void OnEndTimerEvent()
	{
	}

	public void OnRunningTimerEvent(float curTimer)
	{
	}

	public void OnStartTimerEvent()
	{
	}

	public void OnStopTimerEvent()
	{
	}
}
