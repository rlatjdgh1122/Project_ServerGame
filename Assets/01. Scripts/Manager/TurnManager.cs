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
    /// �̹� ���� �����ϴ� ��Ʈ��ũ ������Ʈ�� ��� Spawn���� Start�� ���� �����
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

        //���� ���� ���
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
        //���� ������� �����̼� �ǰ�
        _turnIdx = (_turnIdx == (int)TurnType.End - 1) ? 0 : _turnIdx + 1;

        _turnTypeNV.Value = (TurnType)_turnIdx;
    }


}
