using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// -1을 쓰기 위해서 sbyte로 지정
/// </summary>
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
    public TurnType StartType = TurnType.None;
    public bool IsMyTurn => _turnTypeNV?.Value == _myType;

    private NetworkVariable<TurnType> _turnTypeNV = new();
    private int _turnIdx = 0;

    private TurnType _myType = TurnType.None;

    public override void OnNetworkSpawn()
    {
        OnResiter();
    }

    private void OnResiter()
    {
        _turnTypeNV.OnValueChanged += OnChangedValue;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();

        _turnTypeNV.OnValueChanged -= OnChangedValue;
    }

    public void SetTurnType(TurnType turnType)
    {
        _myType = turnType;
    }

    public void GameStart()
    {
        Debug.Log("게임시작");
        _turnIdx = (int)StartType;
        _turnTypeNV.Value = StartType;

        OnChangedValue(StartType, StartType);

    }

    private void OnChangedValue(TurnType prevValue, TurnType newValue)
    {
        Debug.Log($"턴 체인지 : {prevValue == TurnType.None}");
        if (prevValue == TurnType.None) return;

        SignalHub.OnChangedTurnEvent?.Invoke(prevValue, newValue);

        //나의 턴일 경우
        if (IsMyTurn)
        {
            SignalHub.OnMyTurnEvent?.Invoke();

        } //end if

        else
        {
            SignalHub.OnNotMyTurnEvent?.Invoke();

        }//end if
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
