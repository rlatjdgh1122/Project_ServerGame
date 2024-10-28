using ExtensionMethod.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer
{
    private readonly Dictionary<int, object> _nameToDataDic = new();
    private readonly Dictionary<int, string> _uniqueKeyToNameDic = new();
    private readonly Dictionary<int, object> _uniqueKeyToDataDic = new();

    public void AddData(string name, int uniqeKey, object value)
    {
        _nameToDataDic.TryAdd(name.GetHashCode(), value);
        _uniqueKeyToNameDic.TryAdd(uniqeKey, name);
        _uniqueKeyToDataDic.TryAdd(uniqeKey, value);
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

    public string GetName(int uniqueKey)
    {
        return _uniqueKeyToNameDic[uniqueKey];
    }

    public void UpdateData(int uniqueKey, object newData)
    {
        if (_uniqueKeyToDataDic.ContainsKey(uniqueKey))
        {
            _uniqueKeyToDataDic[uniqueKey] = newData;

        } //end if
    }
}