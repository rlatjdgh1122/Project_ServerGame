using Seongho.TimerSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameStartTimerUI : ExpansionMonoBehaviour, ITimerEventHandler
{
    public float StartTimer = 3f;
    public TextMeshProUGUI TimerText = null;
    public UnityEvent OnEndTimerEvent = null;

    private ITimer _timer = null;

    private void Start()
    {
        _timer = TimerSystem.CreateTimer("게임시작 타이머", StartTimer, 1);

        TimerSystem.OnResiterTimerEvent(_timer, this);
    }


    public void OnGameStartTimer()
    {
        _timer.StartTimer();
    }

    void ITimerEventHandler.OnRunningTimerEvent(float curTimer)
    {
        int seconds = Mathf.FloorToInt(curTimer); // 소수점 버림
        TimerText.text = $"{seconds}초";
    }

    void ITimerEventHandler.OnEndTimerEvent()
    {
        TimerText.text = "게임 시작!";
        OnEndTimerEvent?.Invoke();

        CoroutineUtil.CallWaitForSeconds(1f, () => TimerText.text = "");
    }

    private void OnDestroy()
    {
        TimerSystem.RemoveResiterTimerEvent(_timer, this);
    }
}
