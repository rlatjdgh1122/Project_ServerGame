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
		//등록한 라벨들을 데이터에 저장해줌
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
	/// 설정한 라벨들을 딕셔너리에 매핑해주는 함수
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
	/// 키와 에셋을 매핑해주는 함수
	/// </summary>
	private Object Mapping<T>(string key)
	{
		Object result = null;
		AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

		// 완료 시점에 실행할 내용 callback으로 등록
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
				Debug_S.LogError($"중복된 키를 발견하였습니다. 키 : {key}, 이름 : {(data.Result as Object).name}");

			} //end else

		};

		return result;
	}
}
