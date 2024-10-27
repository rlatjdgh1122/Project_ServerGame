using System.Collections.Generic;
using System;
using UnityEngine;
public struct DataBinding<T> where T : IEquatable<T>
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                //Debug.Log("2 : " + _value.GetHashCode());
                DataBindingManager.Instance.UpdateBinding(0, this);
            } //end if
        }
    }

    private int aa;
    public DataBinding(T value)
    {
        _value = value;

        aa = 0; //�̷������� ���� �Ҵ������
        aa = GetHashCode(); //GetHashCode���� ��ü�� �ִ� �Լ��� �� �� �ִ�.

    }
}