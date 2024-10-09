using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager : IDisposable
{

	private const int MAX_CONNECTIONS = 2;

	private Allocation _allocation;
	private string _joinCode;
	private string _lobbyId;

	public NetworkServer NetServer { get; private set; }
	public string JoinCode => _joinCode;

	public event Action<GameData, ulong> OnPlayerConnect;
	public event Action<GameData, ulong> OnPlayerDisconnect;
	public event Action<string> OnRoomCreated;

	public async Task<bool> StartHostAsync(string lobbyName, GameData userData, bool roomState = false)
	{

		try
		{

			_allocation = await Relay.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
			_joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);

			var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
			var relayServerData = new RelayServerData(_allocation, "dtls");
			transport.SetRelayServerData(relayServerData);

			CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
			lobbyOptions.Data = new Dictionary<string, DataObject>()
			{

				{

					"JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Public, value: _joinCode)

				},

				{

					"UserName", new DataObject(DataObject.VisibilityOptions.Public, value: userData.playerName)

				},

				{

					"Players", new DataObject(visibility: DataObject.VisibilityOptions.Public, value: "1")

				},

			};

			lobbyOptions.IsPrivate = roomState;

			Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_CONNECTIONS, lobbyOptions);
			_lobbyId = lobby.Id;

			HostSingle.Instance.StartCoroutine(HeartbeatLobby(10));

			NetServer = new NetworkServer(NetworkManager.Singleton);
			NetServer.OnClientJoinEvent += HandleClientJoin;
			NetServer.OnClientLeftEvent += HandleClientLeft;

			string userJson = JsonUtility.ToJson(userData);

			NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(userJson);
			NetworkManager.Singleton.StartHost();

			OnRoomCreated?.Invoke(_joinCode);

			return true;

		}
		catch (Exception ex)
		{

			Debug.LogException(ex);
			return false;

		}

	}

	private void HandleClientJoin(GameData gameData, ulong clientId)
	{

		OnPlayerConnect?.Invoke(gameData, clientId);

	}

	private void HandleClientLeft(GameData gameData, ulong clientId)
	{

		OnPlayerDisconnect?.Invoke(gameData, clientId);

	}

	public void Dispose()
	{

		ShutdownAsync();

	}

	public async Task ShutdownAsync()
	{

		try
		{

			if (!string.IsNullOrEmpty(_lobbyId))
			{

				if (HostSingle.Instance != null)
				{
					HostSingle.Instance.StopCoroutine(nameof(HeartbeatLobby));
				}

				try
				{
					await LobbyService.Instance.DeleteLobbyAsync(_lobbyId);
				}
				catch (LobbyServiceException ex)
				{
					Debug.LogError(ex);
				}
			}

			NetServer.OnClientLeftEvent -= HandleClientLeft;
			NetServer.OnClientJoinEvent -= HandleClientJoin;
			_lobbyId = string.Empty;
			NetServer?.Dispose();

			OnPlayerConnect = null;
			OnPlayerDisconnect = null;

		}
		catch (Exception ex)
		{

			Debug.LogError(ex);

		}


		NetworkManager.Singleton.Shutdown();


	}

	public void ChangeLobbyState(bool isLocked)
	{

		LobbyService.Instance.UpdateLobbyAsync(_lobbyId, new UpdateLobbyOptions() { IsLocked = isLocked, IsPrivate = isLocked });

	}

	public void UpdateLobby()
	{

		var o = new UpdateLobbyOptions();
		o.Data = new Dictionary<string, DataObject>()
		{

			{

				"JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Public, value: _joinCode)

			},

			{

				"UserName", new DataObject(DataObject.VisibilityOptions.Member, value: "")

			},

			{

				"Players", new DataObject(visibility: DataObject.VisibilityOptions.Public, value: NetworkManager.Singleton.ConnectedClients.Count.ToString())

			},

		};

		LobbyService.Instance.UpdateLobbyAsync(_lobbyId, o);

	}

	private IEnumerator HeartbeatLobby(float time)
	{

		var timer = new WaitForSecondsRealtime(time);

		while (true)
		{

			Lobbies.Instance.SendHeartbeatPingAsync(_lobbyId);

			yield return timer;

		}

	}

}
