using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{

    public NetworkServer(NetworkManager networkManager)
    {

        _networkManager = networkManager;
        _networkManager.ConnectionApprovalCallback += ApprovalChack;
        _networkManager.OnServerStarted += OnServerReady;

    }


    private Dictionary<ulong, string> _clientToAuthContainer = new();
    private Dictionary<string, UserData> _authIdToUserDataContainer = new();
    private NetworkManager _networkManager;

    public event Action<string, ulong> OnClientJoinEvent;
    public event Action<string, ulong> OnClientLeftEvent;

    private void ApprovalChack(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {

        string json = Encoding.UTF8.GetString(req.Payload);
        var userData = JsonUtility.FromJson<UserData>(json);

        _clientToAuthContainer.Add(req.ClientNetworkId, userData.authId);
        _authIdToUserDataContainer.Add(userData.authId, userData);

        res.Approved = true;
        res.CreatePlayerObject = false;

        OnClientJoinEvent?.Invoke(userData.authId, req.ClientNetworkId);

    }

    private void OnServerReady()
    {

        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;

    }

    private void OnClientDisconnect(ulong clientId)
    {

        if (_clientToAuthContainer.TryGetValue(clientId, out var authID))
        {
            _clientToAuthContainer.Remove(clientId);
            _authIdToUserDataContainer.Remove(authID);
            OnClientLeftEvent?.Invoke(authID, clientId);
        }

    }

    public UserData? GetUserDataByClientID(ulong clientID)
    {
        if (_clientToAuthContainer.TryGetValue(clientID, out string authID))
        {

            if (_authIdToUserDataContainer.TryGetValue(authID, out UserData data))
            {

                return data;

            }

        }

        return null;

    }

    public UserData? GetUserDataByAuthID(string authID)
    {
        if (_authIdToUserDataContainer.TryGetValue(authID, out UserData data))
        {

            return data;

        }

        return null;
    }

    public void SetUserDataByClientId(ulong clientId, UserData userData)
    {

        if (_clientToAuthContainer.TryGetValue(clientId, out string authID))
        {

            if (_authIdToUserDataContainer.ContainsKey(authID))
            {

                _authIdToUserDataContainer[authID] = userData;

            }

        }

    }

    public void Dispose()
    {

        if (_networkManager == null) return;

        _networkManager.ConnectionApprovalCallback -= ApprovalChack;
        _networkManager.OnServerStarted -= OnServerReady;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        if (_networkManager.IsListening)
        {

            _networkManager.Shutdown();

        }

    }

}
