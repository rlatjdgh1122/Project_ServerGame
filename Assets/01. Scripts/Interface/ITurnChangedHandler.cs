using UnityEngine;

public interface ITurnChangedHandler : IInterfaceSetUpHandler
{
    void IInterfaceSetUpHandler.IStart()
    {
        SignalHub.OnChangedTurnEvent += OnTurnChanged;
        SignalHub.OnMyTurnEvent += OnMyTurn;
    }

    void IInterfaceSetUpHandler.IDestroy()
    {
        SignalHub.OnChangedTurnEvent -= OnTurnChanged;
        SignalHub.OnMyTurnEvent -= OnMyTurn;
    }

    public void OnTurnChanged(TurnType prevType, TurnType newType)
    {

    }

    public void OnMyTurn()
    {

    }
}
