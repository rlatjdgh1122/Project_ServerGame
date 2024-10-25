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

public interface IUIView //실제로 텍스트라던가 뭔가 데이터들을 보여주는 것
{
    public void UpdateData(object data); //데이터가 변경될 경우
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


//데이터를 수정한다하면 이 데이터와 바인딩 되어있는 컨테이너에게 데이터를 넘겨줌
public static class UIController //최상단 무모
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

//UIView에게 데이터를 전달해주는 매개체, 하나의 패널을 담당함, 안에 UIContainer가 들어있을 수도 있음
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
