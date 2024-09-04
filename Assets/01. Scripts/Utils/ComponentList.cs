using ExtensionMethod.List;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ComponentList
{
    private List<Component> list = new();

    public ComponentList(Component[] components)
    {
        list.TryClear();

        list = components.ToList();
    }

    public T Find<T>()
    {
        foreach (var item in list)
        {
            if (item is T result)
                return result;
        }

        Debug.LogError($"{typeof(T).Name}�� ������Ʈ�� ã�� �� �����ϴ�.");

        return default;
    }

    public List<T> FindAll<T>()
    {
        List<T> results = new();

        foreach (var item in list)
        {
            if (item is T result)
                results.Add(result);
        }

        return results;
    }
}
