using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] private Player _playerPrefab = null;
    [SerializeField] private TurnType _myType = TurnType.None;

    public void SetMyType(TurnType type)
    {
        _myType = type;
    }

    public TurnType GetMyType()
    {
        return _myType;
    }

    public Player SpawnPlayer(ulong id)
    {
        var player = Instantiate(_playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

        return player;

    }

}
