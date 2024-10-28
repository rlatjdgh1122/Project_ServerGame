using ExtensionMethod.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seongho.BindingSystem
{
    public class DataContainer
    {
        //이름과 데이터를 연결하는 변수
        private readonly Dictionary<int, object> _nameToDataDic = new();

        //데이터의 고유키와 이름을 연결하는 변수
        private readonly Dictionary<int, string> _uniqueKeyToNameDic = new();

        //데이터의 고유키와 데이터를 연결하는 변수
        private readonly Dictionary<int, object> _uniqueKeyToDataDic = new();

        /// <summary>
        /// 데이터를 추가하는 함수
        /// </summary>
        public void AddData(string name, int uniqeKey, object value)
        {
            _nameToDataDic.TryAdd(name.GetHashCode(), value);
            _uniqueKeyToNameDic.TryAdd(uniqeKey, name);
            _uniqueKeyToDataDic.TryAdd(uniqeKey, value);
        }

        /// <summary>
        /// 이름을 통해 캐싱되어 있는 데이터를 전달하는 함수
        /// </summary>
        public DataBinding<T> GetData<T>(string name) where T : IEquatable<T>
        {
            if (_nameToDataDic.TryGetValue(name.GetHashCode(), out var data))
            {
                return data.Cast<DataBinding<T>>();
            }

            Debug.LogError($"키 : {name} 를 가진 데이터는 없습니다.");
            return default;
        }

        /// <summary>
        /// 고유키를 통해 해당 데이터의 이름을 전달하는 함수
        /// </summary>
        public string GetName(int uniqueKey)
        {
            return _uniqueKeyToNameDic[uniqueKey];
        }

        /// <summary>
        /// 데이터의 정보를 업데이트하는 함수
        /// </summary>
        public void UpdateData(int uniqueKey, object newData)
        {
            if (_uniqueKeyToDataDic.ContainsKey(uniqueKey))
            {
                _uniqueKeyToDataDic[uniqueKey] = newData;

            } //end if
        }
    }
}
