using Seongho.InputSystem;
using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
    public GameModeType GameMode = GameModeType.Competition;

    /// <summary>
    /// LobbyManager에서 성공적으로 게임을 시작할 때 실행
    /// </summary>
    public void OnGameInit()
    {
        if (!IsHost) return;

        //호스트가 플레이어들을 생성해준다.
        GameSpawnManager.Instance.SpawnPlayers(GameMode);

        GameInitServerRpc();
    }

    /// <summary>
    /// TimerManager에서 타이머가 다 지났을 경우 실행
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
        //모든 클라에게 게임이 시작 되었다는 것을 알림
        TurnManager.Instance.GameStart();

        SignalHub.OnGameStartEvent?.Invoke();
    }

}
