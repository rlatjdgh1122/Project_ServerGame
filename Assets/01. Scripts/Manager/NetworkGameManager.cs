using Seongho.InputSystem;
using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
    public GameModeType GameMode = GameModeType.Competition;

    /// <summary>
    /// LobbyManager���� ���������� ������ ������ �� ����
    /// </summary>
    public void OnGameInit()
    {
        if (!IsHost) return;

        //ȣ��Ʈ�� �÷��̾���� �������ش�.
        GameSpawnManager.Instance.SpawnPlayers(GameMode);

        GameInitServerRpc();
    }

    /// <summary>
    /// TimerManager���� Ÿ�̸Ӱ� �� ������ ��� ����
    /// </summary>
    public void GameStart()
    {
        if (!IsHost) return;

        GameStartServerRpc();
    }

    [ServerRpc]
    private void GameInitServerRpc()
    {
        GameInitClientRpc();
    }

    [ClientRpc]
    private void GameInitClientRpc()
    {
        SignalHub.OnGameInitEvent?.Invoke();
    }


    [ServerRpc]
    private void GameStartServerRpc()
    {
        GameStartClientRpc();
    }

    [ClientRpc]
    private void GameStartClientRpc()
    {
        //��� Ŭ�󿡰� ������ ���� �Ǿ��ٴ� ���� �˸�
        TurnManager.Instance.GameStart();

        SignalHub.OnGameStartEvent?.Invoke();
    }

}
