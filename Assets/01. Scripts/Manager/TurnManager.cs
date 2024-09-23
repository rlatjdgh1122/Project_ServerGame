using Mono.Cecil.Cil;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public enum TurnType : byte
{
    None = 0,
    Red,
    Blue,
    Yellow,
    Green

}

public class TurnManager : NetworkBehaviour
{
    private NetworkVariable<TurnType> _turnTypeNV = null;

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
            _turnTypeNV.Value = TurnType.Red;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _turnTypeNV.Value = TurnType.Blue;
        }
    }

    private void OnChangedValue(TurnType previousValue, TurnType newValue)
    {
        SignalHub.OnChangedTurnEvent?.Invoke(previousValue, newValue);
    }

    public TurnType GetTurnTarget()
    {
        return _turnTypeNV.Value;
    }

    public void SetTurnTarget(TurnType turnType)
    {
        _turnTypeNV.Value = turnType;
    }


}
