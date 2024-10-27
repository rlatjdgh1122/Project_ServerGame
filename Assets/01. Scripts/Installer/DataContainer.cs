using ExtensionMethod.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    private readonly Dictionary<int, object> _nameToDataDic = new();
    private readonly Dictionary<int, string> _hashCodeToNameDic = new();
    private readonly Dictionary<int, object> _hashCodeToDataDic = new();

    public void AddData(string name, object value)
    {
        Debug.Log($"2 : {value.GetHashCode()}");

        _nameToDataDic.TryAdd(name.GetHashCode(), value);
        _hashCodeToNameDic.TryAdd(value.GetHashCode(), name);
        _hashCodeToDataDic.TryAdd(value.GetHashCode(), value);
    }

    public DataBinding<T> GetData<T>(string name) where T : IEquatable<T>
    {
        if (_nameToDataDic.TryGetValue(name.GetHashCode(), out var data))
        {
            return data.Cast<DataBinding<T>>();
        }

        Debug.LogError($"키 : {name} 를 가진 데이터는 없습니다.");
        return default;
    }

    public string GetData(int hashCode)
    {
        return _hashCodeToNameDic[hashCode];

    }

    public void UpdateData(int hashCode, object newData)
    {
        if (_hashCodeToDataDic.ContainsKey(hashCode))
        {
            _hashCodeToDataDic[hashCode] = newData;
        }
    }
}