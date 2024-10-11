using System;
using Unity.Netcode;
using UnityEngine;

//NetworkMonoSingleton<UserDataController>
public class UserDataController : ExpansionNetworkBehaviour
{
	//private ILobbyManager _lobbyUI = null;

	private NetworkList<UserData> _userDataList;

	private Action<UserData> OnAddUserEvent = null;
	private Action<UserData> OnRemoveUserEvent = null;
	private Action<UserData> OnValueChangedUserEvent = null;

	public void Awake()
	{
		_userDataList = new();

		//_lobbyUI = GetComponent<ILobbyManager>();
	}

	public void OnResiter(IUserDataEventHandler handler)
	{
		OnAddUserEvent += handler.OnAddUser;
		OnRemoveUserEvent += handler.OnRemoveUser;
		OnValueChangedUserEvent += handler.OnValueChangedUser;
	}

	public void RemoveResiter(IUserDataEventHandler handler)
	{
		OnAddUserEvent -= handler.OnAddUser;
		OnRemoveUserEvent -= handler.OnRemoveUser;
		OnValueChangedUserEvent -= handler.OnValueChangedUser;
	}


	public override void OnNetworkSpawn()
	{
		if (IsClient)
		{
			_userDataList.OnListChanged += HandleUserListChanged;

			//������ �ִ� �༮�鵵 �߰�
			foreach (var user in _userDataList)
			{
				HandleUserListChanged(new NetworkListEvent<UserData>
				{
					Type = NetworkListEvent<UserData>.EventType.Add,
					Value = user
				});
			}

		} //end if

		if (IsServer)
		{
			//_lobbyUI.OnHostSpawn();

			GameData data = HostSingle.Instance.NetServer.GetUserDataByClientID(NetworkManager.Singleton.LocalClientId);
			HandleUserJoin(data, NetworkManager.Singleton.LocalClientId);

			HostSingle.Instance.NetServer.OnClientJoinEvent += HandleUserJoin;
			HostSingle.Instance.NetServer.OnClientLeftEvent += HandleUserLeft;
		} // end if
	}

	public override void OnNetworkDespawn()
	{
		//_lobbyUI.OnColorConfireEvent -= ColorConfirm;

		if (IsClient)
		{
			//_lobbyUI.OnClientDespawn();

			_userDataList.OnListChanged -= HandleUserListChanged;
		}

		if (IsServer)
		{
			//_lobbyUI.OnHostDespawn();

			HostSingle.Instance.NetServer.OnClientLeftEvent -= HandleUserLeft;
			HostSingle.Instance.NetServer.OnClientJoinEvent -= HandleUserJoin;
		}
	}


	public UserData GetUserData(ulong clientID)
	{
		return _userDataList[FindIndex(clientID)];
	}

	public void ColorConfirm(TurnType turn)
	{
		SelectColorServerRpc(turn, NetworkManager.Singleton.LocalClientId);
	}

	[ServerRpc(RequireOwnership = false)]
	//�̸� ������ ��Ʈ��ũ ������Ʈ�̱⿡ RequireOwnership�� false��
	public void SelectColorServerRpc(TurnType turn, ulong clientID)
	{
		int idx = FindIndex(clientID);
		var oldData = _userDataList[idx];

		_userDataList[idx] = new UserData
		{
			clientId = clientID,
			playerName = oldData.playerName,
			authId = oldData.authId,
			turnType = turn,
		};

	}

	[ServerRpc(RequireOwnership = false)]
	public void GameStartServerRpc()
	{

	}

	/// <summary>
	/// ó�� ���� �� ����Ǵ� �Լ�
	/// </summary>
	/// <param name="data"></param>
	/// <param name="clientID"></param>
	private void HandleUserJoin(GameData data, ulong clientID)
	{
		int idx = FindIndex(clientID);
		if (idx >= 0) return; //�̹� �����ϴ� �����̴� ����

		var newUser = new UserData
		{
			clientId = clientID,
			playerName = data.playerName,
			authId = data.authId,
			turnType = TurnType.None,
		};

		_userDataList.Add(newUser);
	}

	private void HandleUserLeft(GameData data, ulong clientID)
	{
		if (_userDataList == null) return;

		UserData result = default;

		foreach (var user in _userDataList)
		{
			if (user.clientId != clientID) continue;

			result = user;
			break;

		} //end foreach

		if (!result.Equals(default))
		{
			_userDataList.Remove(result);

		} //end if
		else
		{
			Debug.LogError($"{clientID} ������ ���� : {result.playerName}");

		} //end else
	}

	private void HandleUserListChanged(NetworkListEvent<UserData> evt)
	{
		switch (evt.Type)
		{
			case NetworkListEvent<UserData>.EventType.Add:
				//_lobbyUI.OnAddUser(evt.Value);
				OnAddUserEvent?.Invoke(evt.Value);
				break;
			case NetworkListEvent<UserData>.EventType.Remove:
				//_lobbyUI.OnRemoveUser(evt.Value);
				OnRemoveUserEvent?.Invoke(evt.Value);
				break;
			case NetworkListEvent<UserData>.EventType.Value:
				//_lobbyUI.OnValueChangedUser(evt.Value);
				OnValueChangedUserEvent?.Invoke(evt.Value);
				break;
		}
	}


	private int FindIndex(ulong clientID)
	{
		for (int i = 0; i < _userDataList.Count; ++i)
		{
			if (_userDataList[i].clientId != clientID) continue;

			return i;
		}

		return -1;
	}

}
