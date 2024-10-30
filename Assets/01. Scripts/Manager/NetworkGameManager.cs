using Seongho.InputSystem;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public GameModeType GameMode = GameModeType.Competition;

	public void GameStart()
	{
		if (!IsHost) return;

		//ȣ��Ʈ�� �÷��̾���� �������� ��
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
		//��� Ŭ�󿡰� ������ ���� �Ǿ��ٴ� ���� �˸�
		TurnManager.Instance.GameStart();

		SignalHub.OnGameStartEvent?.Invoke();
	}

}
