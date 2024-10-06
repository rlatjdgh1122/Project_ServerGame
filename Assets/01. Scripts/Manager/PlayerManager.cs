using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerManager : NetworkMonoSingleton<PlayerManager>
{
    [SerializeField] private Player _playerPrefab = null;
    [SerializeField] private TurnType _myType = TurnType.None;

    public void SetTurnType(TurnType type)
    {
        if (!IsOwner) return;

        _myType = type;
        UserData? data = HostSingle.Instance.NetServer.GetUserDataByClientID(OwnerClientId);

        UserData newData = new UserData
        {
            authId = data.Value.authId,
            nickName = data.Value.nickName,
            turnType = type,
        };

        HostSingle.Instance.NetServer.SetUserDataByClientId(OwnerClientId, newData);
    }

    public Player SpawnPlayer(ulong id)
    {
        var player = Instantiate(_playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

        return player;

    }

}
