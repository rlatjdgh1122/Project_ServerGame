using ExtensionMethod.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Unity.Collections;
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

public abstract class UIView : MonoBehaviour, IUIView
{
    public void UpdateData(object data)
    {

    }
}

public interface IUIContainerable { }
public interface IUIContainer<T> : IUIContainerable where T : struct
{
    public void DataBinding(in DataContainer<T> dataContainer);
}


/*//�����͸� �����Ѵ��ϸ� �� �����Ϳ� ���ε� �Ǿ��ִ� �����̳ʿ��� �����͸� �Ѱ���
public static class UIController //�ֻ�� ����
{
    private static Queue<UIContainer> _UIDatas = new();
    private static UIContainer _prevUI = null;
    private static UIContainer _curUI = null;
}*/







public class AA : MonoBehaviour
{

    //[GetBinding("Coin")] //intŸ���� Coin ������ ���� ����ɶ����� ȣ���
    public void DataModify(in int coin)
    {
        
    }

}

/*//UIView���� �����͸� �������ִ� �Ű�ü, �ϳ��� �г��� �����, �ȿ� UIContainer�� ������� ���� ����
public class UIContainer : MonoBehaviour, IUIContainer<int>
{
    public void DataBinding(in DataContainer<int> dataContainer)
    {
        int data = 0;

        dataContainer.GetData("Coin").UpdateValue(result => data = result);


    }

}*/
