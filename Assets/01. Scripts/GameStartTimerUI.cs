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
        _timer = TimerSystem.CreateTimer("���ӽ��� Ÿ�̸�", StartTimer, 1);

        TimerSystem.OnResiterTimerEvent(_timer, this);
    }


    public void OnGameStartTimer()
    {
        _timer.StartTimer();
    }

    void ITimerEventHandler.OnRunningTimerEvent(float curTimer)
    {
        int seconds = Mathf.FloorToInt(curTimer); // �Ҽ��� ����
        TimerText.text = $"{seconds}��";
    }

    void ITimerEventHandler.OnEndTimerEvent()
    {
        TimerText.text = "���� ����!";
        OnEndTimerEvent?.Invoke();

        CoroutineUtil.CallWaitForSeconds(1f, () => TimerText.text = "");
    }

    private void OnDestroy()
    {
        TimerSystem.RemoveResiterTimerEvent(_timer, this);
    }
}
