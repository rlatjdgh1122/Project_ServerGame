using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class GameData
{
	public string playerName;
	public string authId;

	public ArraySegment<byte> Serialize()	
	{
		//�ϴ� 1024 ��û�صΰ�
		ArraySegment<byte> segment = SendBufferHelper.Open(1024);
		Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //���׸�Ʈ�� span���� ����ְ�
		ushort count = 0;
		bool success = true;

		//����. string�� Length�� ���� Length�� �ƴ϶� ����Ʈ�� ��ȯ�ϸ� ���ڴ� 2����Ʈ�� ���´�
		ushort nameLen = (ushort)Encoding.UTF8.GetByteCount(playerName);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length), nameLen); //���� ����ϰ�
		count += sizeof(ushort);

		byte[] nameByte = Encoding.UTF8.GetBytes(playerName);
		Array.Copy(nameByte, 0, segment.Array, segment.Offset + count, nameByte.Length);
		count += nameLen;

		ushort userAuthIdLen = (ushort)Encoding.UTF8.GetByteCount(authId);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), userAuthIdLen); //���� ����ϰ�
		count += sizeof(ushort);

		byte[] userAuthIdByte = Encoding.UTF8.GetBytes(authId);
		Array.Copy(userAuthIdByte, 0, segment.Array, segment.Offset + count, userAuthIdByte.Length);
		count += userAuthIdLen;

		if (!success)
		{
			Debug.LogError("��Ŷ ����ȭ ����");
			return null;
		}

		return SendBufferHelper.Close(count);
	}

	public void Deserialize(byte[] payload)
	{
		ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(payload, 0, payload.Length);
		ushort count = 0;
		ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count)); //�տ� 2����Ʈ
		count += sizeof(ushort);
		playerName = Encoding.UTF8.GetString(s.Slice(count, nameLen));
		count += nameLen;

		ushort userAuthLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
		count += sizeof(ushort);
		authId = Encoding.UTF8.GetString(s.Slice(count, userAuthLen));
		count += userAuthLen;
	}
}

public class NetworkServer : IDisposable
{
	public NetworkServer(NetworkManager networkManager)
	{

		_networkManager = networkManager;
		_networkManager.ConnectionApprovalCallback += ApprovalChack;
		_networkManager.OnServerStarted += OnServerReady;
	}


	private Dictionary<ulong, string> _clientToAuthContainer = new();
	private Dictionary<string, GameData> _authIdToUserDataContainer = new();
	private NetworkManager _networkManager;

	public event Action<GameData, ulong> OnClientJoinEvent;            //GameData, clientId
	public event Action<GameData, ulong> OnClientLeftEvent;            //GameData, clientId

	private void ApprovalChack(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
	{
		string json = Encoding.UTF8.GetString(req.Payload);
		var userData = JsonUtility.FromJson<GameData>(json);

		_clientToAuthContainer.TryAdd(req.ClientNetworkId, userData.authId);
		_authIdToUserDataContainer.TryAdd(userData.authId, userData);

		res.Approved = true;
		res.CreatePlayerObject = false;

		OnClientJoinEvent?.Invoke(userData, req.ClientNetworkId);

	}

	private void OnServerReady()
	{

		_networkManager.OnClientDisconnectCallback += OnClientDisconnect;

	}

	private void OnClientDisconnect(ulong clientId)
	{
		if (_clientToAuthContainer.TryGetValue(clientId, out var authId))
		{
			if (_authIdToUserDataContainer.TryGetValue(authId, out var userData))
			{
				_clientToAuthContainer.Remove(clientId);
				_authIdToUserDataContainer.Remove(authId);

				//������ ���� ��� ���ӵ����Ϳ� Ŭ���̾�Ʈ ���̵� �Ѱ� ��������
				OnClientLeftEvent?.Invoke(userData, clientId);
			} //end if


		} //end if

	}

	public GameData GetUserDataByClientID(ulong clientID)
	{
		if (_clientToAuthContainer.TryGetValue(clientID, out string authID))
		{

			if (_authIdToUserDataContainer.TryGetValue(authID, out GameData data))
			{

				return data;

			}

		}

		return null;

	}

	public GameData GetUserDataByAuthID(string authID)
	{
		if (_authIdToUserDataContainer.TryGetValue(authID, out GameData data))
		{

			return data;

		}

		return null;
	}

	public void SetUserDataByClientId(ulong clientId, GameData userData)
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
