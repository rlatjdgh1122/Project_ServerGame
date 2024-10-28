using System;
using System.Collections.Generic;
using UnityEngine;
namespace Seongho.BindingSystem
{
    public class DataBindingManager : Singleton<DataBindingManager>
    {
        //Type형태로 데이터들을 캐싱해놓은 변수
        private readonly Dictionary<Type, DataContainer> _bindingDatas = new();

        //Name과 Type형태로 메서드를 캐싱해놓은 변수
        private readonly Dictionary<(string name, Type type), List<Action<object>>> _bindingMethods = new();

        /// <summary>
        /// 이름과 고유키를 가지고 데이터를 캐싱하는 함수
        /// </summary>
        public void RegisterDataBinding(string name, int uniqueKey, object dataBinding)
        {
            Type bindingType = dataBinding.GetType();

            if (!_bindingDatas.ContainsKey(bindingType))
            {
                // 해당 type이 처음 바인딩된 경우 DataContainer을 생성한다.
                _bindingDatas[bindingType] = new DataContainer();

            } //end if

            DataContainer container = _bindingDatas[bindingType];

            //DataContainer에 데이터를 추가한다.
            container.AddData(name, uniqueKey, dataBinding);

        }

        /// <summary>
        /// 데이터가 변경될 경우 호출되는 함수
        /// </summary>
        public void UpdateDataBinding<T>(int uniqueKey, DataBinding<T> dataBinding) where T : IEquatable<T>
        {
            if (_bindingDatas.TryGetValue(typeof(DataBinding<T>), out var container))
            {
                //DataContainer에서 데이터를 변경해준다.
                container.UpdateData(uniqueKey, dataBinding);

                //데이터와 캐싱된 이름을 가져온다.
                string name = container.GetName(uniqueKey);
                Type type = typeof(T);

                //메서드를 호출해준다.
                UpdateMethodBinding(name, type, dataBinding);

            } //end if
        }

        public void RegisterMethodBinding(string name, Type type, Action<object> method)
        {
            var key = (name, type);
            if (!_bindingMethods.ContainsKey(key))
            {
                // 해당 key가 처음 바인딩된 경우 새로운 메서드를 담는 리스트를 생성한다.
                _bindingMethods[key] = new List<Action<object>>();

            } //end if

            //데이터 추가
            _bindingMethods[key].Add(method);
        }

        /// <summary>
        /// 데이터가 변경될 경우 자동으로 호출되는 함수
        /// </summary>
        private void UpdateMethodBinding<T>(string name, Type dataType, DataBinding<T> dataBinding) where T : IEquatable<T>
        {
            var key = (name, dataType);

            if (_bindingMethods.TryGetValue(key, out var actions))
            {
                //name과 dataType을 키로 갖는 모든 메서드를 호출해준다.
                foreach (var action in actions)
                {
                    action(dataBinding.Value);

                } //end foreach
            } //end if

            else
            {
                Debug.LogError($" 이름 : {name}], Type : [{dataType}] 같은 데이터를 찾을 수 없습니다.");

            } //end if
        }

        /// <summary>
        /// 바인딩 되지 않아도 데이터를 호출할 수 있는 함수
        /// </summary>
        public DataBinding<T> GetDataBinding<T>(string name) where T : IEquatable<T>
        {
            if (_bindingDatas.TryGetValue(typeof(T), out var container))
            {
                return container.GetData<T>(name);

            } //end if

            return default;
        }
    }
}
