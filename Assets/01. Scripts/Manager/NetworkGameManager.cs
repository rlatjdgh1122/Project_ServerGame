using Seongho.InputSystem;
using UnityEngine;

public class NetworkGameManager : NetworkMonoSingleton<NetworkGameManager>
{
	public void GameStart()
	{
		Debug.Log(IsServer);
		if (!IsServer) return;
		//플레이어들을 생성해준 뒤
		SpawnManager.Instance.SpawnPlayers();

		//게임 시작
		TurnManager.Instance.GameStart();

		SignalHub.OnGameStartEvent?.Invoke();
	}

}
