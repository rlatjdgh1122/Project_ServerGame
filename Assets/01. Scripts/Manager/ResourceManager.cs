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
		//����� �󺧵��� �����Ϳ� ��������
		foreach (var label in _loadLabels)
		{
			_curLabelName = label.labelString;

			var handle = Addressables.LoadResourceLocationsAsync(label.labelString, typeof(Sprite));
			handle.Completed += OnLoadLabelListCompleted;
			Debug_S.Log($"�� : {_curLabelName}");
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
	/// �󺧿� ���ԵǾ� �ִ� ������ ���������� ������ ��� �۾��� ó�����ִ� �Լ�
	/// </summary>
	private void OnLoadLabelListCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
	{
		foreach (IResourceLocation result in handle.Result)
		{
			DataModify<Object>(result.PrimaryKey, Mapping);
			Debug_S.Log("Ȯ��");

		} //end foreach
	}

	/// <summary>
	/// Ű�� ������ �������ִ� �Լ�
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

		// �Ϸ� ������ ������ ���� callback���� ���
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
				Debug_S.LogError($"�ߺ��� Ű�� �߰��Ͽ����ϴ�. Ű : {key}, �̸� : {(data.Result as Object).name}");

			} //end else

		};

	}
}
