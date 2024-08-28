using System;
using Unity.Netcode;

[Serializable]
public struct PrefabData : INetworkSerializable, IEquatable<PrefabData>
{

    public string prefabKey;

    public bool Equals(PrefabData other)
    {

        return prefabKey.Equals(other.prefabKey);

    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {

        serializer.SerializeValue(ref prefabKey);

    }

}