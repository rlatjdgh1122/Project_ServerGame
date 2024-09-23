

public delegate void ChangedTurnEvent(TurnType prevType, TurnType newValue);


public static class SignalHub
{
    public static ChangedTurnEvent OnChangedTurnEvent = null;
}