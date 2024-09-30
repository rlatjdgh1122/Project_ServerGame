using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

/// <summary>
/// ��巹����� ���ҽ��Ŵ���
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
	[SerializeField] private List<AssetLabelReference> _defaultLoadLabels = new();                //�ν����Ϳ��� ������ �󺧵��� ����
	private MultiKeyDictionary<string, Type, IDataContainer> _labelToDataContainerDic = new();    //�󺧰� Ÿ�Կ� �ش�Ǵ� ���µ��� �����ϴ� ��ųʸ�
	private Dictionary<Type, IDataContainer> _typeToDataContainerDic = new();                     //Ÿ�Կ� �ش�Ǵ� ���µ��� �����ϴ� ��ųʸ�

	private string _curLabelName = "";       //���� �� �̸��� �������ش�.
	private MethodInfo _cachedMethod = null; //���÷��� �ߺ��� ���� ���� �̸� ĳ�����ش�.

	private int _curCount = 0;
	private int _targetCount = 0;

	public override void Awake()
	{
		base.Awake();

		if (_cachedMethod == null)
		{
			_cachedMethod = typeof(ResourceManager).GetMethod("DataModify", BindingFlags.NonPublic | BindingFlags.Instance);

		} //end if

		Init();
	}

	private void Init()
	{
		OnRegisterLabel("Face_Default");
		/*//����� �󺧵��� �������ش�.
		foreach (var label in _defaultLoadLabels)
		{
			OnRegisterLabel(label.labelString);

		} //end foreach*/
	}

	/// <summary>
	/// Ÿ�԰� Ű�� �ش�Ǵ� ������ �����´�.
	/// </summary>
	public T GetAsset<T>(string key) where T : Object
	{
		if (_typeToDataContainerDic.TryGetValue(typeof(T), out var con1))
		{
			return con1.GetData(key) as T;

		} //end if

		else if (_typeToDataContainerDic.TryGetValue(typeof(GameObject), out var con2))
		{
			if ((con2.GetData(key) as GameObject).TryGetComponent(out T result))
			{
				return result;

			}//end try_componet
			else
			{
				Debug_S.LogError($"�̸� : ({key})���� {typeof(T)}�� ���ԵǾ� ���� �ʽ��ϴ�.");

			}//end else

		} //end try_value

		Debug_S.LogError($"Ÿ�� : ({typeof(T)}), �̸� : ({key})�� ã�� �� �����ϴ�.");
		return null;
	}



	public List<T> GetAssetsByLabelName<T>(string labelName) where T : Object
	{
		List<T> list = new List<T>();

		if (_labelToDataContainerDic.TryGetValue(labelName, out var data))
		{
			if (data.TryGetValue(typeof(T), out var container))
			{
				foreach (var item in container.GetAllValues())
				{
					if (item is T result)
					{
						list.Add(result);

					} //end if

				} //end foreach

			} //end if
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
		_targetCount += handle.Result.Count;

		foreach (IResourceLocation result in handle.Result)
		{
			//Debug_S.Log($"ResourceType : {result.ResourceType}");

			//���ʸ� �޼���� ������ �ÿ� Ÿ���� �����Ǳ� ������
			//���÷����� ����Ͽ� result.ResourceType Ÿ�Կ� ���� ��Ÿ�Կ� �������� �������ش�.
			_cachedMethod?.MakeGenericMethod(result.ResourceType)
						 .Invoke(this, new object[] { result.PrimaryKey });
		} //end foreach
	}

	/// <summary>
	/// ���÷����� �� ĳ������ �Լ�, �����͸� �ε����ְ� ��ųʸ��� �߰����ش�.
	/// </summary>
	private void DataModify<T>(string key) where T : class
	{
		AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

		// �Ϸ� ������ ������ ���� callback���� ���
		handle.Completed += (data) =>
		{
			T result = data.Result as T;

			AddAsset(key, result);
			AddLabelAsset(_curLabelName, key, result);
			Addressables.Release(data);

			_curCount++;
			if(_curCount == _targetCount)
			{
				SignalHub.OnAssetLoadCompetedEvent?.Invoke();

			}//end if
		};
	}

	private void AddAsset<T>(string key, T value) where T : class
	{
		if (!_typeToDataContainerDic.ContainsKey(typeof(T)))
		{
			_typeToDataContainerDic.Add(typeof(T), new DataContainer<T>());
		}

		DataContainer<T> container = _typeToDataContainerDic[typeof(T)] as DataContainer<T>;
		container.AddData(key, value);
	}

	private void AddLabelAsset<T>(string label, string key, T value) where T : class
	{
		if (!_labelToDataContainerDic.ContainsKey(label, typeof(T)))
		{
			_labelToDataContainerDic.Add(label, typeof(T), new DataContainer<T>());
		}
		DataContainer<T> container = _labelToDataContainerDic[label, typeof(T)] as DataContainer<T>;
		container.AddData(key, value);
	}

}
