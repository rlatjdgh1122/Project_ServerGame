using System;
using Unity.Collections;
using Unity.Netcode;

public struct UserData : INetworkSerializable, IEquatable<UserData>
{
	public ulong clientId;
	public FixedString32Bytes playerName;
	public FixedString32Bytes authId;
	public TurnType turnType;

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref clientId);
		serializer.SerializeValue(ref playerName);
		serializer.SerializeValue(ref authId);
		serializer.SerializeValue(ref turnType);
	}

	public bool Equals(UserData other)
	{
		return clientId == other.clientId
			&& playerName == other.playerName
			&& authId == other.authId
			&& turnType == other.turnType;
	}
}