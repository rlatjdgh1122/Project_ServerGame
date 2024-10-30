using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IUITarget
{

}

public static class UIManager<T> where T : IUITarget
{
    private static Dictionary<(Type type, string name), T> _uiTargetDatas = new();


    public static T GetUI(string name)
    {
        var key = (typeof(T), name);

        if (_uiTargetDatas.TryGetValue(key, out var ui))
        {
            return ui;

        } //end if

        Debug.LogError($"Type : [{typeof(T)}], name : [{name}]�� �ش��ϴ� UI�� ã�� �� �����ϴ�.");

        return default;
    }

    public static void ResisterUI(T ui, string name)
    {
        var key = (typeof(T), name);

        if (!_uiTargetDatas.ContainsKey(key))
        {
            _uiTargetDatas.Add(key, ui);

        } //end if

        else
        {
            Debug.LogError($"Type : [{typeof(T)}], name : [{name}]�� �ش��ϴ� UI�� �ߺ��Ǿ����ϴ�.");

        } //end else
    }
}

public class GetUI : MonoBehaviour
{
    private void Start()
    {
        UIManager<IUIWarningText>.GetUI("wqer").ShowText("�α����� �� �� �����ϴ�.", 3f);

    }
}
