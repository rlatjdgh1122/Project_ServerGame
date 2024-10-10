using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawnManager : NetworkMonoSingleton<GameSpawnManager>
{
	[SerializeField] private Transform _startPos = null;
	[SerializeField] private Transform _currentPos = null;

	public override void Awake()
	{
		_currentPos = _startPos;
	}

	public void ChangedSpawnPosition(Transform pos)
	{
		_currentPos = pos;
	}

	public void ReStartSpawnPosition(Transform pos)
	{
		_currentPos = _startPos;
	}

	public void SpawnPlayers(GameModeType modeType)
	{
		foreach (var id in NetworkManager.ConnectedClientsIds)
		{
			Player player = SpawnerManager.Instance.SpawnPlayer(id, modeType);
			player.SetPostion(_currentPos);


		} //end foreach

	}
}
