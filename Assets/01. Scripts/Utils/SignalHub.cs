

public delegate void ChangedTurnEvent(TurnType prevType, TurnType newValue);
public delegate void MyTurnEvent();

public static class SignalHub
{
    public static ChangedTurnEvent OnChangedTurnEvent = null;
    public static MyTurnEvent OnMyTurnEvent = null;
}