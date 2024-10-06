using System;
using System.Collections.Generic;
using Unity.Netcode;

public readonly struct SerializeKeyValuePair<TKey, TValue> : INetworkSerializable, IEquatable<SerializeKeyValuePair<TKey, TValue>>
	where TKey : unmanaged, INetworkSerializable, IEquatable<TKey>
	where TValue : unmanaged, INetworkSerializable, IEquatable<TValue>
{
	public TKey Key { get; }
	public TValue Value { get; }

	public SerializeKeyValuePair(TKey key, TValue value)
	{
		Key = key;
		Value = value;
	}

	public bool Equals(SerializeKeyValuePair<TKey, TValue> other)
	{
		return true;
	}

	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		throw new NotImplementedException();
	}
}

public class NetworkDictionary<TKey, TValue> : NetworkVariableBase
	where TKey : unmanaged, INetworkSerializable,IEquatable<TKey>
	where TValue : unmanaged, INetworkSerializable, IEquatable<TValue>
{
	private List<SerializeKeyValuePair<TKey, TValue>> internalList = new List<SerializeKeyValuePair<TKey, TValue>>();

	public event Action<TKey, TValue> OnDictionaryChanged = null;

	public NetworkDictionary() { }

	// Get value by key
	public TValue this[TKey key]
	{
		get
		{
			foreach (var pair in internalList)
			{
				if (pair.Key.Equals(key))
				{
					return pair.Value;
				}
			}
			return default;
		}
		set => AddOrUpdate(key, value);
	}

	// Add or update a key-value pair
	public void AddOrUpdate(TKey key, TValue value)
	{
		bool found = false;
		for (int i = 0; i < internalList.Count; i++)
		{
			if (internalList[i].Key.Equals(key))
			{
				internalList[i] = new SerializeKeyValuePair<TKey, TValue>(key, value);
				found = true;
				break;
			}
		}

		if (!found)
		{
			internalList.Add(new SerializeKeyValuePair<TKey, TValue>(key, value));
		}

		OnDictionaryChanged?.Invoke(key, value);
		SetDirty(true);  // Mark as dirty to sync
	}

	// Remove a key-value pair
	public bool Remove(TKey key)
	{
		for (int i = 0; i < internalList.Count; i++)
		{
			if (internalList[i].Key.Equals(key))
			{
				internalList.RemoveAt(i);
				SetDirty(true);  // Mark as dirty to sync
				return true;
			}
		}
		return false;
	}

	// Check if key exists
	public bool ContainsKey(TKey key)
	{
		foreach (var pair in internalList)
		{
			if (pair.Key.Equals(key))
			{
				return true;
			}
		}
		return false;
	}

	public override void ResetDirty()
	{
		SetDirty(false);
	}

	public override void WriteDelta(FastBufferWriter writer)
	{
		// Serialize the KeyValuePair list
		writer.WriteValueSafe((ushort)internalList.Count);
		foreach (var pair in internalList)
		{
			writer.WriteValueSafe(pair.Key);
			writer.WriteValueSafe(pair.Value);
		}
	}

	public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
	{
		internalList.Clear();
		ushort count;
		reader.ReadValueSafe(out count);

		for (int i = 0; i < count; i++)
		{
			TKey key;
			TValue value;
			reader.ReadValueSafe(out key);
			reader.ReadValueSafe(out value);
			internalList.Add(new SerializeKeyValuePair<TKey, TValue>(key, value));
		}
	}

	public override void WriteField(FastBufferWriter writer)
	{
		WriteDelta(writer);
	}

	public override void ReadField(FastBufferReader reader)
	{
		ReadDelta(reader, true);
	}
}
