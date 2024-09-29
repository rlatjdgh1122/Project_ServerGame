using ExtensionMethod.Object;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField] private List<AssetLabelReference> _defaultLoadLabels = new();                                   //�ν����Ϳ��� ������ �󺧵��� ����
    private Dictionary<string, (Type type, IDataContainer dataContainer)> _labelToTypeAndDataContainerDic = new();   //�󺧰� Ÿ�Կ� �ش�Ǵ� ���µ��� �����ϴ� ��ųʸ�
    private Dictionary<Type, IDataContainer> _typeToDataContainerDic = new();                                        //Ÿ�Կ� �ش�Ǵ� ���µ��� �����ϴ� ��ųʸ�

    private string _curLabelName = "";       //���� �� �̸��� �������ش�.
    private MethodInfo _cachedMethod = null; //���÷��� �ߺ��� ���� ���� �̸� ĳ�����ش�.

    public override void Awake()
    {
        base.Awake();

        //ó�� �������� ��� ĳ�����ش�.
        if (_cachedMethod == null)
        {
            _cachedMethod = typeof(ResourceManager).GetMethod("DataModify", BindingFlags.NonPublic | BindingFlags.Instance);

        } //end if

        Setting();
    }

    private void Setting()
    {
        //����� �󺧵��� �������ش�.
        foreach (var label in _defaultLoadLabels)
        {
            OnRegisterLabel(label.labelString);
        }
    }

    /// <summary>
    /// Ÿ�԰� Ű�� �ش�Ǵ� ������ �����´�.
    /// </summary>
    public T GetAsset<T>(string key) where T : Object
    {
        if (_typeToDataContainerDic.TryGetValue(typeof(T), out var dataContainer))
        {
            return dataContainer.GetData(key).Cast<T>();
        }

        Debug_S.LogError($"Ÿ�� : ({typeof(T)}), �̸� : ({key})�� ã�� �� �����ϴ�.");
        return null;
    }


    public List<T> GetAssetsByLabelName<T>(string labelName) where T : Object
    {
        List<T> list = new List<T>();

        if (_labelToTypeAndDataContainerDic.TryGetValue(labelName, out var dataContainer))
        {
            if(dataContainer.)
            foreach (var item in dataContainer.GetAllValues())
            {
                if (item is T result)
                {
                    list.Add(result);
                }

            } //end foreach
        } //end if
        return list;
    }

    /// <summary>
    /// ���� ������ش�.
    /// ��� ����) ��Ų�� �� ��� �������
    public void OnRegisterLabel(string label)
    {
        _curLabelName = label;

        var handle = Addressables.LoadResourceLocationsAsync(label);
        handle.Completed += OnLoadLabelListCompleted;
    }



    /// <summary>
    /// �󺧿� ���ԵǾ� �ִ� ������ ���������� ������ ��� �۾��� ó�����ִ� �Լ�
    /// </summary>
    private void OnLoadLabelListCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        foreach (IResourceLocation result in handle.Result)
        {
            //���ʸ� �޼���� ������ �ÿ� Ÿ���� �����Ǳ� ������
            //���÷����� ����Ͽ� result.ResourceType Ÿ�Կ� ���� ��Ÿ�Կ� �������� �������ش�.
            _cachedMethod?.MakeGenericMethod(result.ResourceType)
                         .Invoke(this, new object[] { result.PrimaryKey });
        } //end foreach
    }

    /// <summary>
    /// ĳ�� �Լ�, �����͸� �ε����ְ� ��ųʸ��� �߰����ش�.
    /// </summary>
    private void DataModify<T>(string key) where T : class
    {
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

        // �Ϸ� ������ ������ ���� callback���� ���
        handle.Completed += (data) =>
        {
            T result = (T)data.Result;
            Add(key, result);

            Addressables.Release(data);

        };
    }

    private void Add<T>(string key, T value) where T : class
    {
        if (!_typeToDataContainerDic.ContainsKey(typeof(T)))
        {
            _typeToDataContainerDic.Add(typeof(T), new DataContainer<T>());
        }

        DataContainer<T> container = _typeToDataContainerDic[typeof(T)] as DataContainer<T>;
        container.AddData(key, value);
    }
}
