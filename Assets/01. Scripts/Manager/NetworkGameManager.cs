using Seongho.InputSystem;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public void GameStart()
	{
		if (!IsServer) return;

		//호스트가 플레이어들을 생성해준 뒤
		SpawnManager.Instance.SpawnPlayers();

		GameStartServerRpc();
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
