

#region �ΰ���

public delegate void GameStartEvent();
public delegate void GameEndEvent();

public delegate void ChangedTurnEvent(TurnType prevType, TurnType newValue);
public delegate void MyTurnEvent();

#endregion

#region ���·ε� �Ϸ�

public delegate void AssetLoadCompletedEvent();

#endregion

public static class SignalHub
{
    public static GameStartEvent OnGameInitEvent = null;   //���Ӿ��� �ʱ�ȭ �� ��
    public static GameStartEvent OnGameStartEvent = null;  //������ ������ ��
    public static GameEndEvent OnGameEndEvent = null;      //������ ���� ��

    public static ChangedTurnEvent OnChangedTurnEvent = null;
    public static MyTurnEvent OnMyTurnEvent = null;
    public static MyTurnEvent OnNotMyTurnEvent = null;

    public static AssetLoadCompletedEvent OnAssetLoadCompetedEvent = null;
}