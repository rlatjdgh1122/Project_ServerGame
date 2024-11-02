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
            Debug.Log($"�̹� �����ִ� â�Դϴ�.");
            return;

        } //end if

        // ���� ȭ���� �����ش�.
        if (_uiStack.Count > 0)
        {
            _uiStack.Peek().Hide();

        } //end if

        //���� UI�� ��ü���ش�.
        _currentUI = ui;

        _uiStack.Push(ui);
        ui.Show();
    }

    public void Pop()
    {
        if (_uiStack.Count <= 0) return;

        //����â�� �ݾ��ش�.
        IUIPop currnet = _uiStack.Pop();
        currnet.Hide();
        _currentUI = null;

        //���� â�� ����â���� ���� �� �����ش�.
        _currentUI = _uiStack.Peek();
        _currentUI.Show();
    }

    public IUIPop GetCurrentPopupUI()
    {
        if(_currentUI == null)
        {
            Debug.Log("���� �����ִ� â�� �����ϴ�.");

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

        Debug.Log($"Type : [{typeof(T)}] �� ã�� �� �����ϴ�.");

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

public class GetUI : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.GetController<UIPopController>().GetCurrentPopupUI();
    }
}
