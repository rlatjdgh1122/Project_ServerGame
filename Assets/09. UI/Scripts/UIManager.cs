using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIPop : IUITarget
{
    public void Pop();
    public void Push();
}

public interface IUITarget
{

}

public interface IUIController
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

/// <summary>
/// UI�� �ִϸ��̼��� ����ϴ� Ŭ����, UIElement Ŭ�������� ����
/// </summary>
public class UIAnimator
{
    private MonoBehaviour monoBehaviour = null;
    private Transform transform = null;
    private Coroutine coroutine = null;

    public UIAnimator(MonoBehaviour @object)
    {
        monoBehaviour = @object;
        transform = @object.transform;
    }

    public void DoScale(Vector3 startSize, Vector3 endSize, float duration)
    {
        //������ �ڷ�ƾ�� �ð��Ǿ��ٸ� ���� ��
        if (coroutine != null)
        {
            monoBehaviour.StopCoroutine(coroutine);

        } //end if

        // ���� Coroutine ����
        coroutine = monoBehaviour.StartCoroutine(Corou_DoScale(startSize, endSize, duration));
    }

    private IEnumerator Corou_DoScale(Vector3 startSize, Vector3 targetSize, float duration)
    {
        transform.localScale = startSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startSize, targetSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetSize;
        coroutine = null; // �ִϸ��̼� �Ϸ� �� �ڷ�ƾ ���� �ʱ�ȭ
    }
}


public static class UIController<T> where T : IUIController
{

}

public class UIPopupController : UIElement, IUIController
{

}

public class GetUI : MonoBehaviour
{
    private void Start()
    {
        UIManager<IUIWarningText>.GetUI("wqer").ShowText("�α����� �� �� �����ϴ�.", 3f);

        UIManager<IUIPop>.GetUI("werq").Pop();
        //UIController<UIPopupController>.GetUI("werq").GetCurrentUI;
    }
}
