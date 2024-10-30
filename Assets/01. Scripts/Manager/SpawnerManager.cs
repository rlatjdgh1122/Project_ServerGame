using Unity.Netcode;
using UnityEngine;
public class SpawnerManager : NetworkMonoSingleton<SpawnerManager>
{
    [SerializeField] private Player _playerPrefab = null;
    
    public Player SpawnPlayer(ulong id, GameModeType modeType)
    {
        var player = Instantiate(_playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

        return player;
    }

}
