using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer<T> where T : class
{
	private Dictionary<int, T> _hashCodeToDataDic = new();

	public T GetData(int key)
	{
		return _hashCodeToDataDic[key];
	}

	public void SetData(int key, T value)
	{
		_hashCodeToDataDic.Add(key, value);
	}

	public Dictionary<int, T>.ValueCollection GetValues()
	{
		return _hashCodeToDataDic.Values;
	}
}
