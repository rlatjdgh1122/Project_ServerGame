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
		//�÷��̾���� �������� ��
		SpawnManager.Instance.SpawnPlayers();

		//���� ����
		TurnManager.Instance.GameStart();

		SignalHub.OnGameStartEvent?.Invoke();
	}

}
