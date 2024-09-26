using ExtensionMethod.Object;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

public class ResourceManager : MonoSingleton<ResourceManager>
{
	[SerializeField] private List<AssetLabelReference> _loadLabels = new();
	private Dictionary<string, Object> _keyToObjectDic = new();
	private Dictionary<string, List<Object>> _labelNameToObjectListDic = new();


	private string _curLabelName = "";
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

			var handle = Addressables.LoadResourceLocationsAsync(label.labelString);
			handle.Completed += OnLoadLabelListCompleted;
		}
	}

	public T GetAsset<T>(string key) where T : Object
	{
		if (_keyToObjectDic.TryGetValue(key, out var value))
		{
			return value.Cast<T>();
		}

		else return null;
	}

	public List<T> GetAssets<T>(string labelName) where T : Object
	{
		List<T> list = new List<T>();

		if (!_labelNameToObjectListDic.ContainsKey(labelName))
		{
			foreach (Object item in _labelNameToObjectListDic[labelName])
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
	/// ������ �󺧵��� ��ųʸ��� �������ִ� �Լ�
	/// </summary>
	private void OnLoadLabelListCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
	{
		foreach (IResourceLocation result in handle.Result)
		{
			Object data = Mapping<Object>(result.PrimaryKey);
			_labelNameToObjectListDic[_curLabelName].Add(data);

		} //end foreach
	}

	/// <summary>
	/// Ű�� ������ �������ִ� �Լ�
	/// </summary>
	private Object Mapping<T>(string key)
	{
		Object result = null;
		AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

		// �Ϸ� ������ ������ ���� callback���� ���
		handle.Completed += (data) =>
		{
			if (!_keyToObjectDic.ContainsKey(key))
			{
				result = (Object)data.Result;
				_keyToObjectDic.Add(key, result);

				Addressables.Release(data);

			} //end if

			else
			{
				Debug_S.LogError($"�ߺ��� Ű�� �߰��Ͽ����ϴ�. Ű : {key}, �̸� : {(data.Result as Object).name}");

			} //end else

		};

		return result;
	}
}
