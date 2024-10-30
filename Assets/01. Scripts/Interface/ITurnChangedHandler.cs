using UnityEngine;

public interface ITurnChangedHandler : IInterfaceSetUpHandler
{
    void IInterfaceSetUpHandler.IStart()
    {
        SignalHub.OnChangedTurnEvent += OnTurnChanged;
        SignalHub.OnMyTurnEvent += OnMyTurn;
        SignalHub.OnNotMyTurnEvent += OnNotMyTurn;
    }

    void IInterfaceSetUpHandler.IDestroy()
    {
        SignalHub.OnChangedTurnEvent -= OnTurnChanged;
        SignalHub.OnMyTurnEvent -= OnMyTurn;
        SignalHub.OnNotMyTurnEvent -= OnNotMyTurn;
    }

    public void OnTurnChanged(TurnType prevType, TurnType newType) { }

    public void OnMyTurn() { }

    public void OnNotMyTurn() { }
}
