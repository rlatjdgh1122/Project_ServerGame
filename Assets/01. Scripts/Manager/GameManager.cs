using Seongho.InputSystem;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	[SerializeField] private INPUT_TYPE _initInputType = INPUT_TYPE.Player;

	public override void Awake()
	{
		InputManager.ChangedInputType(_initInputType);
	}

	private void Start()
	{
		GameStart();
	}

	private void GameStart()
	{
		SpawnManager.Instance.SpawnPlayers();

		TurnManager.Instance.GameStart();
	}

}
