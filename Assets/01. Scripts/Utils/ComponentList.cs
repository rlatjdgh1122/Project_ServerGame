using ExtensionMethod.List;
using ExtensionMethod.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentList
{
	private Dictionary<Type, List<Component>> _typeToComponentDic = new();

	public T Find<T>()
	{
		if (_typeToComponentDic.TryGetValue(typeof(T), out var compos))
		{
			return compos[0].Cast<T>();
		}

		Debug.LogError($"{typeof(T).Name}의 컴포넌트를 찾을 수 없습니다.");

		return default;
	}

	public List<T> FindAll<T>()
	{
		if (_typeToComponentDic.TryGetValue(typeof(T), out var compos))
		{
			return compos.Cast<T>().ToList();
		}

		Debug.LogError($"{typeof(T).Name}의 컴포넌트를 찾을 수 없습니다.");

		return null;
	}

	public void Add(Type type, Component compo)
	{
		if (!_typeToComponentDic.ContainsKey(type))
		{
			_typeToComponentDic[type] = new List<Component>();
		}

		_typeToComponentDic[type].Add(compo);
	}
}
