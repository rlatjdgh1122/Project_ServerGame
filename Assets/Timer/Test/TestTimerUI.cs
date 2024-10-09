using Seongho.TimerSystem;
using UnityEngine;
using UnityEngine.UI;

public class TestTimerUI : ExpansionMonoBehaviour, ITimerEventHandler
{
    private ITimer timer = null;

    public Text TimerText = null;

    private void Awake()
    {
        //테스트라는 이름으로 타이머를 생성해주고 타이머를 10초로 지정해준다.
        TimerSystem.CreateTimer("테스트", 10);
    }

    private void Start()
    {
        //이름으로 타이머를 가져온다.
        timer = TimerSystem.GetTimer("테스트");

        //ITimerEventHandler를 상속받은 후 타이머와 이벤트를 연결해준다. (연결해야 이벤트 함수를 사용 가능)
        TimerSystem.OnResiterTimerEvent(timer, this);
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
            timer.AddTime(10);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            timer.ResetTimer();
        }
    }

    /// <summary>
    /// 타이머가 시작 시 호출되는 함수
    /// </summary>
    void ITimerEventHandler.OnStartTimerEvent()
    {
        TimerText.text = "타이머 시작";
    }

    /// <summary>
    /// 타이머가 멈출 시 호출되는 함수
    /// </summary>
    void ITimerEventHandler.OnStopTimerEvent()
    {
        TimerText.text = "타이머 멈춤";
    }

    /// <summary>
    /// 타이머가 끝날 시 호출되는 함수
    /// </summary>
    void ITimerEventHandler.OnEndTimerEvent()
    {
        TimerText.text = "타이머 끝";
    }

    /// <summary>
    /// 타이머를 리셋할 시 호출되는 함수
    /// </summary>
    void ITimerEventHandler.OnResetTimerEvent()
    {
        TimerText.text = $"{timer.GetMaxTime()} 초";
    }

    /// <summary>
    /// 타이머가 작동 중일 시 호출되는 함수
    /// </summary>
    void ITimerEventHandler.OnRunningTimerEvent(float curTimer)
    {
        int seconds = Mathf.FloorToInt(curTimer); // 소수점 버림
        TimerText.text = $"{seconds} 초";
    }
}
