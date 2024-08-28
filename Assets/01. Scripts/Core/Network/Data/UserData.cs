using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public struct UserData : INetworkSerializable
{

    public string nickName;
    public string authId;
    public Color color;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref nickName);
        serializer.SerializeValue(ref authId);
        serializer.SerializeValue(ref color);
    }
}