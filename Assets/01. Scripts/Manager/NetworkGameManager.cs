using Seongho.InputSystem;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public void GameStart()
	{
		if (!IsServer) return;

		//ȣ��Ʈ�� �÷��̾���� �������� ��
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
        //��� Ŭ�󿡰� ������ ���� �Ǿ��ٴ� ���� �˸�
        TurnManager.Instance.GameStart();

        SignalHub.OnGameStartEvent?.Invoke();
    }

}
