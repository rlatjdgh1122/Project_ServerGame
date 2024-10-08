/*using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public readonly struct SerializeKeyValuePair<TKey, TValue> : IEquatable<SerializeKeyValuePair<TKey, TValue>>
	where TKey : unmanaged, IEquatable<TKey>
	where TValue : unmanaged, IEquatable<TValue>
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
		Vector3 a, b;

		return Key.Equals(other.Key) && Value.Equals(other);
	}


	*//*
		public ArraySegment<byte> Serialize()
		{
			//�ϴ� 1024 ��û�صΰ�
			ArraySegment<byte> segment = SendBufferHelper.Open(1024);
			Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count); //���׸�Ʈ�� span���� ����ְ�
			ushort count = 0;
			bool success = true;

			//����. string�� Length�� ���� Length�� �ƴ϶� ����Ʈ�� ��ȯ�ϸ� ���ڴ� 2����Ʈ�� ���´�
			ushort nameLen = (ushort)Encoding.UTF8.GetByteCount(Key);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length), nameLen); //���� ����ϰ�
			count += sizeof(ushort);

			byte[] nameByte = Encoding.UTF8.GetBytes(username);
			Array.Copy(nameByte, 0, segment.Array, segment.Offset + count, nameByte.Length);
			count += nameLen;

			ushort userAuthIdLen = (ushort)Encoding.UTF8.GetByteCount(userAuthId);
			success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), userAuthIdLen); //���� ����ϰ�
			count += sizeof(ushort);

			byte[] userAuthIdByte = Encoding.UTF8.GetBytes(userAuthId);
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
			username = Encoding.UTF8.GetString(s.Slice(count, nameLen));
			count += nameLen;

			ushort userAuthLen = BitConverter.ToUInt16(s.Slice(count, s.Length - count));
			count += sizeof(ushort);
			userAuthId = Encoding.UTF8.GetString(s.Slice(count, userAuthLen));
			count += userAuthLen;
		}*//*
}

public class NetworkDictionary<TKey, TValue> : NetworkVariableBase
	where TKey : unmanaged, IEquatable<TKey>
	where TValue : unmanaged, IEquatable<TValue>
{
	private NativeList<SerializeKeyValuePair<TKey, TValue>> internalList = new NativeList<SerializeKeyValuePair<TKey, TValue>>();

	public event Action<TKey, TValue> OnDictionaryChanged = null;

	public NetworkList<TKey> a;

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
		for (int i = 0; i < internalList.Length; i++)
		{
			//���� ���� �����Ѵٸ� ��ü���ְ�
			if (internalList[i].Key.Equals(key))
			{
				internalList[i] = new SerializeKeyValuePair<TKey, TValue>(key, value);
				found = true;
				break;
			}
		}

		//�������� �ʴٸ� �߰�����
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
		for (int i = 0; i < internalList.Length; i++)
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
		writer.WriteValueSafe((ushort)internalList.Length);
		foreach (var pair in internalList)
		{
			NetworkVariableSerialization<TKey>.Write(writer, ref element.Value);
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
*/