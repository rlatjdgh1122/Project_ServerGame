public interface ITurnChangedHandler : IInterfaceSetUpHandler
{   
    void IInterfaceSetUpHandler.IStart()
    {
        SignalHub.OnChangedTurnEvent += OnTurnChanged;
    }

    void IInterfaceSetUpHandler.IDestroy()
    {
        SignalHub.OnChangedTurnEvent -= OnTurnChanged;
    }

    public void OnTurnChanged(TurnType prevType, TurnType newType);
}
