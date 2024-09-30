using Seongho.InputSystem;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public void GameStart()
	{
		Debug.Log(IsServer);
		if (!IsServer) return;
		//�÷��̾���� �������� ��
		SpawnManager.Instance.SpawnPlayers();

		//���� ����
		TurnManager.Instance.GameStart();

		SignalHub.OnGameStartEvent?.Invoke();
	}

}
