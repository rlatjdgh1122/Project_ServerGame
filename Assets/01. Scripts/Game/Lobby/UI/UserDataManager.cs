using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class UserDataManager : NetworkMonoSingleton<UserDataManager>
{
	public NetworkList<UserData> _userDataList;

	public event Action<UserData> OnAddUserEvent = null;
	public event Action<UserData> OnRemoveUserEvent = null;
	public event Action<UserData> OnValueChangedUserEvent = null;

	public override void Awake()
	{
		_userDataList = new();
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
			GameData data = HostSingle.Instance.NetServer.GetUserDataByClientID(NetworkManager.Singleton.LocalClientId);
			HandleUserJoin(data, NetworkManager.Singleton.LocalClientId);

			HostSingle.Instance.NetServer.OnClientJoinEvent += HandleUserJoin;
			HostSingle.Instance.NetServer.OnClientLeftEvent += HandleUserLeft;
		} // end if
	}

	public override void OnNetworkDespawn()
	{
		if (IsClient)
		{
			_userDataList.OnListChanged -= HandleUserListChanged;	
		}

		if (IsServer)
		{
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
				OnAddUserEvent?.Invoke(evt.Value);
				break;
			case NetworkListEvent<UserData>.EventType.Remove:
				Debug.Log(evt.Value.clientId);
				OnRemoveUserEvent?.Invoke(evt.Value);
				break;
			case NetworkListEvent<UserData>.EventType.Value:
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
