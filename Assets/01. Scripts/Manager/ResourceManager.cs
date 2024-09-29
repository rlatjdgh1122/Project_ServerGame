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
    [SerializeField] private List<AssetLabelReference> _defaultLoadLabels = new();                                   //인스펙터에서 가져올 라벨들을 지정
    private Dictionary<string, (Type type, IDataContainer dataContainer)> _labelToTypeAndDataContainerDic = new();   //라벨과 타입에 해당되는 에셋들을 저장하는 딕셔너리
    private Dictionary<Type, IDataContainer> _typeToDataContainerDic = new();                                        //타입에 해당되는 에셋들을 저장하는 딕셔너리

    private string _curLabelName = "";       //현재 라벨 이름을 저장해준다.
    private MethodInfo _cachedMethod = null; //리플렉션 중복을 막기 위해 미리 캐시해준다.

    public override void Awake()
    {
        base.Awake();

        //처음 실행해줄 경우 캐싱해준다.
        if (_cachedMethod == null)
        {
            _cachedMethod = typeof(ResourceManager).GetMethod("DataModify", BindingFlags.NonPublic | BindingFlags.Instance);

        } //end if

        Setting();
    }

    private void Setting()
    {
        //등록한 라벨들을 실행해준다.
        foreach (var label in _defaultLoadLabels)
        {
            OnRegisterLabel(label.labelString);
        }
    }

    /// <summary>
    /// 타입과 키에 해당되는 에셋을 가져온다.
    /// </summary>
    public T GetAsset<T>(string key) where T : Object
    {
        if (_typeToDataContainerDic.TryGetValue(typeof(T), out var dataContainer))
        {
            return dataContainer.GetData(key).Cast<T>();
        }

        Debug_S.LogError($"타입 : ({typeof(T)}), 이름 : ({key})을 찾을 수 없습니다.");
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
    /// 라벨을 등록해준다.
    /// 사용 예시) 스킨을 살 경우 등록해줌
    public void OnRegisterLabel(string label)
    {
        _curLabelName = label;

        var handle = Addressables.LoadResourceLocationsAsync(label);
        handle.Completed += OnLoadLabelListCompleted;
    }



    /// <summary>
    /// 라벨에 포함되어 있는 에셋을 성공적으로 가져올 경우 작업을 처리해주는 함수
    /// </summary>
    private void OnLoadLabelListCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        foreach (IResourceLocation result in handle.Result)
        {
            //제너릭 메서드는 컴파일 시에 타입을 고정되기 때문에
            //리플렉션을 사용하여 result.ResourceType 타입에 따라 런타입에 동적으로 전달해준다.
            _cachedMethod?.MakeGenericMethod(result.ResourceType)
                         .Invoke(this, new object[] { result.PrimaryKey });
        } //end foreach
    }

    /// <summary>
    /// 캐싱 함수, 데이터를 로드해주고 딕셔너리에 추가해준다.
    /// </summary>
    private void DataModify<T>(string key) where T : class
    {
        AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

        // 완료 시점에 실행할 내용 callback으로 등록
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
