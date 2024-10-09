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
		//일단 1024 요청해두고
		ArraySegment<byte> segment = SendBufferHelper.Open(1024);
		Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //세그먼트를 span으로 찝어주고
		ushort count = 0;
		bool success = true;

		//주의. string은 Length가 실제 Length가 아니라 바이트로 변환하면 글자당 2바이트로 나온다
		ushort nameLen = (ushort)Encoding.UTF8.GetByteCount(playerName);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length), nameLen); //길이 기록하고
		count += sizeof(ushort);

		byte[] nameByte = Encoding.UTF8.GetBytes(playerName);
		Array.Copy(nameByte, 0, segment.Array, segment.Offset + count, nameByte.Length);
		count += nameLen;

		ushort userAuthIdLen = (ushort)Encoding.UTF8.GetByteCount(authId);
		success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), userAuthIdLen); //길이 기록하고
		count += sizeof(ushort);

		byte[] userAuthIdByte = Encoding.UTF8.GetBytes(authId);
		Array.Copy(userAuthIdByte, 0, segment.Array, segment.Offset + count, userAuthIdByte.Length);
		count += userAuthIdLen;

		if (!success)
		{
			Debug.LogError("패킷 직렬화 오류");
			return null;
		}

		return SendBufferHelper.Close(count);
	}

	public void Deserialize(byte[] payload)
	{
		ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(payload, 0, payload.Length);
		ushort count = 0;
		ushort nameLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count)); //앞에 2바이트
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

				//유저가 나갈 경우 게임데이터와 클라이언트 아이디를 넘겨 발행해줌
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
