using Seongho.InputSystem;
using UnityEngine;

public class GameManager : NetworkMonoSingleton<GameManager>
{
	[SerializeField] private INPUT_TYPE _initInputType = INPUT_TYPE.Player;

	public override void Awake()
	{
		InputManager.ChangedInputType(_initInputType);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Y))
		{
			GameStart();
		}
	}

	private void GameStart()
	{
		//플레이어들을 생성해준 뒤
		SpawnManager.Instance.SpawnPlayers();

		//게임 시작
		TurnManager.Instance.GameStart();

		SignalHub.OnGameStartEvent?.Invoke();
	}

}
