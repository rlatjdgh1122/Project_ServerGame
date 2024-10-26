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
                OnValueChanged?.Invoke(value); // ���� ����� �� ȣ��
            }
        }
    }

    public DataBinding(T value)
    {
        _value = value;
        OnValueChanged = null; // �ʱ�ȭ
    }

    public event Action<T> OnValueChanged; // ���� ����� �� ȣ��� �̺�Ʈ
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
        // �ʵ忡 BindingAttribute�� �ִ��� Ȯ��
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
        coin.Value = 1; // ó���� coin ���� 1�� ����
    }

    private void Start()
    {
        coin.Value = 3; // coin ���� 3���� ����
    }
}

public class a : MonoBehaviour, IBinding
{
    [GetBinding("coin")]
    public void GetData(int coin)
    {
        Debug.Log(coin); //ó�� : 1, ���� : 3
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

        Debug.LogError($"Ű : {name} �� ���� �����ʹ� �����ϴ�.");
        return default;
    }
}

public class DataBindingManager : Singleton<DataBindingManager>
{
    private readonly Dictionary<Type, DataContainer> _bindings = new();

    public void RegisterBinding(string name, object dataBinding)
    {
        Type bindingType = dataBinding.GetType();
        if (!_bindings.ContainsKey(bindingType))  // type�� ó�� ���ε��� ���
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

        Debug.LogError($"���ε����� ���� ������: {name}");
        return default;
    }
}
