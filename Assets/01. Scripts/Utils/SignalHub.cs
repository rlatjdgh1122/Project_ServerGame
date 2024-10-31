

#region 인게임

public delegate void GameStartEvent();
public delegate void GameEndEvent();

public delegate void ChangedTurnEvent(TurnType prevType, TurnType newValue);
public delegate void MyTurnEvent();

#endregion

#region 에셋로드 완료

public delegate void AssetLoadCompletedEvent();

#endregion

public static class SignalHub
{
    public static GameStartEvent OnGameInitEvent = null;   //게임씬이 초기화 될 때
    public static GameStartEvent OnGameStartEvent = null;  //게임이 시작할 때
    public static GameEndEvent OnGameEndEvent = null;      //게임이 끝날 때

    public static ChangedTurnEvent OnChangedTurnEvent = null;
    public static MyTurnEvent OnMyTurnEvent = null;
    public static MyTurnEvent OnNotMyTurnEvent = null;

    public static AssetLoadCompletedEvent OnAssetLoadCompetedEvent = null;
}