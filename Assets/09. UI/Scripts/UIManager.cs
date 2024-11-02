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
    public void Show();
    public void Hide();
}

public abstract class UIControllerBase
{
    protected abstract void Init();
}

public class UIPopController : UIControllerBase
{
    private Stack<IUIPop> _uiStack = new();
    private IUIPop _currentUI = null;

    protected override void Init()
    {

    }

    public void Push(IUIPop ui)
    {
        if(_currentUI != null && _currentUI == ui)
        {
            Debug.Log($"이미 열려있는 창입니다.");
            return;

        } //end if

        // 이전 화면을 숨겨준다.
        if (_uiStack.Count > 0)
        {
            _uiStack.Peek().Hide();

        } //end if

        //현재 UI를 교체해준다.
        _currentUI = ui;

        _uiStack.Push(ui);
        ui.Show();
    }

    public void Pop()
    {
        if (_uiStack.Count <= 0) return;

        //현재창을 닫아준다.
        IUIPop currnet = _uiStack.Pop();
        currnet.Hide();
        _currentUI = null;

        //이전 창을 현재창으로 설정 후 보여준다.
        _currentUI = _uiStack.Peek();
        _currentUI.Show();
    }

    public IUIPop GetCurrentPopupUI()
    {
        if(_currentUI == null)
        {
            Debug.Log("현재 켜져있는 창이 없습니다.");

        } //end if
        return _currentUI;
    }

}


public class UIManager : MonoSingleton<UIManager>
{
    private readonly Dictionary<Type, object> _controllerDatas = new();

    public T GetController<T>() where T : UIControllerBase
    {
        if (_controllerDatas.TryGetValue(typeof(T), out object controller))
        {
            return (T)controller;
        }

        Debug.Log($"Type : [{typeof(T)}] 를 찾을 수 없습니다.");

        return null;
    }
}

public abstract class UIElement : ExpansionMonoBehaviour
{
    public UIAnimator UIAnimator = null;

    public virtual void Awake()
    {
        Install();
    }

    private void Install()
    {
        UIAnimator = new(this);
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

public class GetUI : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.GetController<UIPopController>().GetCurrentPopupUI();
    }
}
