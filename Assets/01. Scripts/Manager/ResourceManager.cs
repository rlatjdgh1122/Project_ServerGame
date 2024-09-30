using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

/// <summary>
/// 어드레서블용 리소스매니저
/// </summary>
public class ResourceManager : MonoSingleton<ResourceManager>
{
	[SerializeField] private List<AssetLabelReference> _defaultLoadLabels = new();                //인스펙터에서 가져올 라벨들을 지정
	private MultiKeyDictionary<string, Type, IDataContainer> _labelToDataContainerDic = new();    //라벨과 타입에 해당되는 에셋들을 저장하는 딕셔너리
	private Dictionary<Type, IDataContainer> _typeToDataContainerDic = new();                     //타입에 해당되는 에셋들을 저장하는 딕셔너리

	private string _curLabelName = "";       //현재 라벨 이름을 저장해준다.
	private MethodInfo _cachedMethod = null; //리플렉션 중복을 막기 위해 미리 캐시해준다.

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
		/*//등록한 라벨들을 실행해준다.
		foreach (var label in _defaultLoadLabels)
		{
			OnRegisterLabel(label.labelString);

		} //end foreach*/
	}

	/// <summary>
	/// 타입과 키에 해당되는 에셋을 가져온다.
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
				Debug_S.LogError($"이름 : ({key})에는 {typeof(T)}가 포함되어 있지 않습니다.");

			}//end else

		} //end try_value

		Debug_S.LogError($"타입 : ({typeof(T)}), 이름 : ({key})을 찾을 수 없습니다.");
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
		_targetCount += handle.Result.Count;

		foreach (IResourceLocation result in handle.Result)
		{
			//Debug_S.Log($"ResourceType : {result.ResourceType}");

			//제너릭 메서드는 컴파일 시에 타입을 고정되기 때문에
			//리플렉션을 사용하여 result.ResourceType 타입에 따라 런타입에 동적으로 전달해준다.
			_cachedMethod?.MakeGenericMethod(result.ResourceType)
						 .Invoke(this, new object[] { result.PrimaryKey });
		} //end foreach
	}

	/// <summary>
	/// 리플렉션할 때 캐싱해줄 함수, 데이터를 로드해주고 딕셔너리에 추가해준다.
	/// </summary>
	private void DataModify<T>(string key) where T : class
	{
		AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(key);

		// 완료 시점에 실행할 내용 callback으로 등록
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
