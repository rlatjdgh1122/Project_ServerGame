using ExtensionMethod.Object;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class ResourceManager : MonoSingleton<ResourceManager>
{
	[SerializeField] private List<AssetLabelReference> _loadLabels = new();
	private Dictionary<Type, DataContainer<object>> _typeToDataContainerDic = new();

	private string _curLabelName = "";

	public void Add<T>(T value) where T : class
	{
		DataContainer<T> container = _typeToDataContainerDic[typeof(T)] as DataContainer<T>;
		_typeToDataContainerDic.Add(typeof(T), container.SetData(value, value));
	}

	public T GetValue<T>(string key) where T : class
	{
		DataContainer<T> container = _typeToDataContainerDic[typeof(T)] as DataContainer<T>;
		return container.GetData(key.GetHashCode());
	}


	public override void Awake()
	{
		base.Awake();

		Setting();
	}

	private void Setting()
	{
		//등록한 라벨들을 데이터에 저장해줌
		foreach (var label in _loadLabels)
		{
			_curLabelName = label.labelString;

			var handle = Addressables.LoadResourceLocationsAsync(label.labelString, typeof(Sprite));
			handle.Completed += OnLoadLabelListCompleted;
			Debug_S.Log($"라벨 : {_curLabelName}");
		}
	}

	public T GetAsset<T>(string key) where T : Object
	{
		if (_typeToDataContainerDic.TryGetValue(typeof(T), out var value))
		{
			return value.GetData(key).Cast<T>();
		}

		else return null;
	}

	public List<T> GetAssetsByLabelName<T>(string labelName) where T : Object
	{
		List<T> list = new List<T>();

		if (_typeToDataContainerDic.TryGetValue(typeof(T), out var listValue))
		{
			foreach (Object item in listValue.)
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
	/// 라벨에 포함되어 있는 에셋을 성공적으로 가져올 경우 작업을 처리해주는 함수
	/// </summary>
	private void OnLoadLabelListCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
	{
		foreach (IResourceLocation result in handle.Result)
		{
			DataModify<Object>(result.PrimaryKey, Mapping);
			Debug_S.Log("확인");

		} //end foreach
	}

	/// <summary>
	/// 키와 에셋을 매핑해주는 함수
	/// </summary>
	private void Mapping(Object data)
	{
		Debug.Log(data.GetType());
		if (!_labelNameToObjectListDic.ContainsKey(_curLabelName))
		{
			_labelNameToObjectListDic.Add(_curLabelName, new List<Object>());

		} //end if

		if (!_labelNameToObjectListDic[_curLabelName].Contains(data))
		{
			_labelNameToObjectListDic[_curLabelName].Add(data);

		} //end if
	}


	private void DataModify<T>(string key, Action<Object> action)
	{
		AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

		// 완료 시점에 실행할 내용 callback으로 등록
		handle.Completed += (data) =>
		{
			if (!_keyToObjectDic.ContainsKey(key))
			{
				Object result = (Object)data.Result;
				action?.Invoke(result);
				_keyToObjectDic.Add(key, result);

				Addressables.Release(data);

			} //end if
			else
			{
				Debug_S.LogError($"중복된 키를 발견하였습니다. 키 : {key}, 이름 : {(data.Result as Object).name}");

			} //end else

		};

	}
}
