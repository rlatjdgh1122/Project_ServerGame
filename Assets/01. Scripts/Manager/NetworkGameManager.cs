using Seongho.InputSystem;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public void GameStart()
	{
		if (!IsServer) return;
		//�÷��̾���� �������� ��
		SpawnManager.Instance.SpawnPlayers();

		//���� ����
		TurnManager.Instance.GameStart();

		SignalHub.OnGameStartEvent?.Invoke();
	}

}
