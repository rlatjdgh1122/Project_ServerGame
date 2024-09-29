using ExtensionMethod.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataContainer
{
    public void AddData(string key, object data);
    public object GetData(string key);
    IEnumerable<object> GetAllValues();
}

public class DataContainer<T> : IDataContainer where T : class
{
    private Dictionary<int, T> _hashCodeToDataDic = new();

    public void AddData(string key, object data)
    {
        _hashCodeToDataDic.Add(key.GetHashCode(), data.Cast<T>());
    }

    public object GetData(string key)
    {
        return _hashCodeToDataDic[key.GetHashCode()];
    }

    public IEnumerable<object> GetAllValues()
    {
        foreach (var value in _hashCodeToDataDic.Values)
        {
            yield return value;
        }
    }
}
