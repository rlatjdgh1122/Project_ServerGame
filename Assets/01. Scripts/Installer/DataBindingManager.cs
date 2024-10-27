using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.BindingSystem;

public class DataBindingManager : Singleton<DataBindingManager>
{
    private readonly Dictionary<Type, DataContainer> _bindings = new();
    private readonly Dictionary<string, List<Action<object>>> _bindingMethods = new();

    public void RegisterBinding(string name, object dataBinding)
    {
        Type bindingType = dataBinding.GetType();

        if (!_bindings.ContainsKey(bindingType))  // type이 처음 바인딩된 경우
        {
            _bindings[bindingType] = new DataContainer();

        } //end if

        DataContainer container = _bindings[bindingType];

        container.AddData(name, dataBinding);
    }

    public void UpdateBinding<T>(int hashCode, DataBinding<T> dataBinding) where T : IEquatable<T>
    {
        if (_bindings.TryGetValue(typeof(DataBinding<T>), out var container))
        {
            container.UpdateData(hashCode, dataBinding);
            Debug.Log($"3 : {hashCode}");

            string name = container.GetData(hashCode);

            if (_bindingMethods.TryGetValue(name, out var actions))
            {
                foreach (var action in actions)
                {
                    action(dataBinding.Value);
                }
            }
            else
            {
                Debug.LogError($"No binding methods found for: {name}");
            }

        } //end if
    }


    public DataBinding<T> GetBinding<T>(string name) where T : IEquatable<T>
    {
        if (_bindings.TryGetValue(typeof(DataBinding<T>), out var container))
        {
            return container.GetData<T>(name);
        }

        Debug.LogError($"바인딩되지 않은 데이터: {name}");

        return default;
    }

    public void AAA(string name, Action<object> action)
    {
        if (!_bindingMethods.ContainsKey(name))
        {
            _bindingMethods[name] = new List<Action<object>>();
        }

        // �޼��带 ���ٷ� ĳ��
        _bindingMethods[name].Add(action);
    }
}