using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Jobs;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AppController : MonoSingleton<AppController>
{

    [SerializeField] private ClientSingle _clientPrefab = null;
    [SerializeField] private HostSingle _hostPrefab = null;

    public UnityEvent OnInitCompletedEvent = null;
    public UnityEvent OnInitFailedEvent = null;

    public async void Start()
    {
        await UnityServices.InitializeAsync();

        var state = await AuthenticationWrapper.DoAuth(3);

        if (state != AuthState.Authenticated)
        {

            OnInitFailed();
            return;

        }

        HostSingle host = Instantiate(_hostPrefab, transform);
        host.CreateHost();

        ClientSingle client = Instantiate(_clientPrefab, transform);
        client.CreateClient();

        OnInitComplete();

    }

    private void OnInitFailed()
    {
        OnInitFailedEvent?.Invoke();
    }

    private void OnInitComplete()
    {

        OnInitCompletedEvent?.Invoke();

    }

    public async Task<bool> StartHostAsync(string username, string lobbyName, bool roomState = false)
    {

        return await HostSingle.Instance.GameManager.StartHostAsync(lobbyName, GetUserData(username), roomState);

    }

    public async Task StartClientAsync(string username, string joinCode)
    {

        await ClientSingle.Instance.GameManager.StartClientAsync(joinCode, GetUserData(username));

    }

    public UserData GetUserData(string username, TurnType turnType = default)
    {

        return new UserData
        {

            nickName = username,
            authId = AuthenticationService.Instance.PlayerId,
            turnType = turnType,

        };

    }

    public async Task<List<Lobby>> GetLobbyList()
    {

        try
        {

            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 20;
            options.Filters = new List<QueryFilter>()
            {

                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"),

            };


            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);
            return lobbies.Results;

        }
        catch (LobbyServiceException ex)
        {

            Debug.LogError(ex);
            return new List<Lobby>();

        }

    }

}
