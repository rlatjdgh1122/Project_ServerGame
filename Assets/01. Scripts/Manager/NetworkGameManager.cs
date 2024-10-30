using Seongho.InputSystem;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public GameModeType GameMode = GameModeType.Competition;

	public void GameStart()
	{
		if (!IsHost) return;

		//호스트가 플레이어들을 생성해준 뒤
		GameSpawnManager.Instance.SpawnPlayers(GameMode);

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
