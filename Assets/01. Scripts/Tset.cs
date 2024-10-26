using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.BindingSystem;

public struct DataBinding<T>
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(value); // 값이 변경될 때 호출
            }
        }
    }

    public DataBinding(T value)
    {
        _value = value;
        OnValueChanged = null; // 초기화
    }

    public event Action<T> OnValueChanged; // 값이 변경될 때 호출될 이벤트
}

public interface IBinding { }

[DefaultExecutionOrder(-100)]
public class BindingInstaller : MonoBehaviour
{
    private void Awake()
    {
        var components = GetComponents<IBinding>();

        foreach (var component in components)
        {
            RegisterBindings(component);
        }
    }

    private void RegisterBindings(IBinding component)
    {
        // 필드에 BindingAttribute가 있는지 확인
        var fields = component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            var bindingAttribute = field.GetCustomAttribute<BindingAttribute>();
            if (bindingAttribute != null)
            {
                var fieldValue = field.GetValue(component);

                DataBindingManager.Instance.RegisterBinding(bindingAttribute.Name, fieldValue);


            }
        }
    }

}

public class TestCompo : MonoBehaviour, IBinding
{
    [Binding("coin")] private DataBinding<int> coin = new(3);
    [Binding("name")] private DataBinding<string> playerName = new("");

    private void Awake()
    {
        coin.Value = 1; // 처음에 coin 값을 1로 설정
    }

    private void Start()
    {
        coin.Value = 3; // coin 값을 3으로 변경
    }
}

public class a : MonoBehaviour, IBinding
{
    [GetBinding("coin")]
    public void GetData(int coin)
    {
        Debug.Log(coin); //처음 : 1, 이후 : 3
    }
}

public class DataContainer
{
    private readonly Dictionary<int, object> _hashKeyToDataDic = new();

    public void AddData(string name, object value)
    {
        _hashKeyToDataDic.TryAdd(name.GetHashCode(), value);
    }

    public DataBinding<T> GetData<T>(string name)
    {
        if (_hashKeyToDataDic.TryGetValue(name.GetHashCode(), out var data) && data is DataBinding<T> binding)
        {
            return binding;
        }

        Debug.LogError($"키 : {name} 를 가진 데이터는 없습니다.");
        return default;
    }
}

public class DataBindingManager : Singleton<DataBindingManager>
{
    private readonly Dictionary<Type, DataContainer> _bindings = new();

    public void RegisterBinding(string name, object dataBinding)
    {
        Type bindingType = dataBinding.GetType();
        if (!_bindings.ContainsKey(bindingType))  // type이 처음 바인딩된 경우
        {
            _bindings[bindingType] = new DataContainer();
        }
        DataContainer container = _bindings[bindingType];

        container.AddData(name, dataBinding);
    }

    public DataBinding<T> GetBinding<T>(string name)
    {
        if (_bindings.TryGetValue(typeof(DataBinding<T>), out var container))
        {
            return ((DataContainer)container).GetData<T>(name);
        }

        Debug.LogError($"바인딩되지 않은 데이터: {name}");
        return default;
    }
}
