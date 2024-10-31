using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// -1�� ���� ���ؼ� sbyte�� ����
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
        Debug.Log("���ӽ���");
        _turnIdx = (int)StartType;
        _turnTypeNV.Value = StartType;

        OnChangedValue(StartType, StartType);

    }

    private void OnChangedValue(TurnType prevValue, TurnType newValue)
    {
        Debug.Log($"�� ü���� : {prevValue == TurnType.None}");
        if (prevValue == TurnType.None) return;

        SignalHub.OnChangedTurnEvent?.Invoke(prevValue, newValue);

        //���� ���� ���
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
        //���� ������� �����̼� �ǰ�
        _turnIdx = (_turnIdx == (int)TurnType.End - 1) ? 0 : _turnIdx + 1;

        _turnTypeNV.Value = (TurnType)_turnIdx;
    }


}
