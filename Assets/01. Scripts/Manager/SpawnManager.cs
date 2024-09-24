using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : NetworkMonoSingleton<SpawnManager>
{
    [SerializeField] private Transform _startPos = null;
    [SerializeField] private Transform _currentPos = null;

    public override void Awake()
    {
        _currentPos = transform;
    }

    public void ChangedSpawnPosition(Transform pos)
    {
        _currentPos = pos;
    }

    public void ReStartSpawnPosition(Transform pos)
    {
        _currentPos = _startPos;
    }

    public void SpawnPlayers()
    {
        foreach (var id in NetworkManager.ConnectedClientsIds)
        {
            Player player = PlayerManager.Instance.SpawnPlayer(id);
            player.SetPostion(_currentPos);

        } //end foreach

    }
}
