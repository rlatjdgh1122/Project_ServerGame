using ExtensionMethod.Object;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public interface IUIPop
{
    public void Pop();
    public void Push();
}

public interface IUIView //������ �ؽ�Ʈ����� ���� �����͵��� �����ִ� ��
{
    public void UpdateData(object data); //�����Ͱ� ����� ���
}

public interface IUIContainerable { }
public interface IUIContainer<T> : IUIContainerable
{
    public void DataBinding(in T data);
}


public abstract class UIView : MonoBehaviour, IUIView
{
    public void UpdateData(object data)
    {

    }
}


//�����͸� �����Ѵ��ϸ� �� �����Ϳ� ���ε� �Ǿ��ִ� �����̳ʿ��� �����͸� �Ѱ���
public static class UIController //�ֻ�� ����
{
    private static Queue<UIContainer> _UIDatas = new();
    private static Dictionary<object, int> _aa = new();
    private static UIContainer _prevUI = null;
    private static UIContainer _curUI = null;

    public static void SetData<T>(T data)
    {
        if (_aa.ContainsKey(data))
        {

        }

    }

    public static void AddUIContrainer(in UIContainer control)
    {

    }

}

//UIView���� �����͸� �������ִ� �Ű�ü, �ϳ��� �г��� �����, �ȿ� UIContainer�� ������� ���� ����
public class UIContainer : MonoBehaviour, IUIContainer<int>
{
    private void Start()
    {
        //UIController.AddUIContrainer(GetInstanceID(), this);
    }

    public void DataBinding(in int data)
    {

    }

    public void Pop()
    {

    }

    public void Push()
    {

    }

    
}
