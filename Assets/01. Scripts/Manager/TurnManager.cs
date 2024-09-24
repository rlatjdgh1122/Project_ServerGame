using Unity.Netcode;
using UnityEngine;

public enum TurnType : sbyte
{
    None = -1,
    Red,
    Blue,
    //Yellow,
    //Green,
    End
}

public class TurnManager : NetworkMonoSingleton<TurnManager>
{
    public bool IsMyTurn => _turnTypeNV?.Value == PlayerManager.Instance.GetMyType();

    private NetworkVariable<TurnType> _turnTypeNV = null;
    private int _turnIdx = -1;

    /// <summary>
    /// 이미 씬에 존재하는 네트워크 오브젝트일 경우 Spawn보다 Start가 먼저 실행됨
    /// </summary>
    private void Start()
    {
        _turnTypeNV = new();

        _turnTypeNV.OnValueChanged += OnChangedValue;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        _turnTypeNV.OnValueChanged -= OnChangedValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnTurnChangedNext();
        }
    }

    private void OnChangedValue(TurnType prevValue, TurnType newValue)
    {
        if (prevValue == TurnType.None) return;

        SignalHub.OnChangedTurnEvent?.Invoke(prevValue, newValue);

        //나의 턴일 경우
        if (IsMyTurn)
        {
            SignalHub.OnMyTurnEvent?.Invoke();

        } //end if
    }

    public TurnType GetTurnTarget()
    {
        return _turnTypeNV.Value;
    }

    public void SetTurnTarget(TurnType turnType)
    {
        _turnTypeNV.Value = turnType;
    }

    public void OnTurnChangedNext()
    {
        //턴이 순서대로 로테이션 되게
        _turnIdx = (_turnIdx == (int)TurnType.End - 1) ? 0 : _turnIdx + 1;

        _turnTypeNV.Value = (TurnType)_turnIdx;
    }


}
