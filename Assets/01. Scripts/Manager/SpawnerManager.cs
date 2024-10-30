using Cinemachine;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class SpawnerManager : NetworkMonoSingleton<SpawnerManager>
{
    [SerializeField] private Player _playerPrefab = null;
    [SerializeField] private PlayerCamera _cameraPrefab = null;

    public Player SpawnPlayer(ulong id, GameModeType modeType)
    {
        var player = Instantiate(_playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

        var camera = Instantiate(_cameraPrefab);
        camera.SetTarget(player, id);

        CameraManager.Instance.Add(id, camera);
        CameraManager.Instance.ShowPlayerCamera(id);

        return player;
    }

}
