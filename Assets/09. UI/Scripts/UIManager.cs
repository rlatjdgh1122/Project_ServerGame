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

        Debug.LogError($"Type : [{typeof(T)}], name : [{name}]에 해당하는 UI를 찾을 수 없습니다.");

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
            Debug.LogError($"Type : [{typeof(T)}], name : [{name}]에 해당하는 UI가 중복되었습니다.");

        } //end else
    }
}

/// <summary>
/// UI의 애니메이션을 담당하는 클래스, UIElement 클래스에서 생성
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
        //기존에 코루틴이 시간되었다면 정지 후
        if (coroutine != null)
        {
            monoBehaviour.StopCoroutine(coroutine);

        } //end if

        // 새로 Coroutine 시작
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
        coroutine = null; // 애니메이션 완료 후 코루틴 상태 초기화
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
        UIManager<IUIWarningText>.GetUI("wqer").ShowText("로그인을 할 수 없습니다.", 3f);

        UIManager<IUIPop>.GetUI("werq").Pop();
        //UIController<UIPopupController>.GetUI("werq").GetCurrentUI;
    }
}
