using System.Collections.Generic;
using System;
using UnityEngine;

public interface IGetUniqeKey
{
    public int UniqeKey { get; }
}

public struct DataBinding<T> : IGetUniqeKey where T : IEquatable<T>
{
    private T _value;
    private readonly int _uniqeKey;

    int IGetUniqeKey.UniqeKey { get => _uniqeKey; }

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;

                //데이터가 변경될 때마다 데이터를 업데이트해준다.
                DataBindingManager.Instance.UpdateDataBinding(_uniqeKey, this);
            } //end if
        }
    }

    public DataBinding(T value)
    {
        _value = value;
        _uniqeKey = default; //이런식으로 값을 할당해줘야
        _uniqeKey = GetHashCode(); //GetHashCode같이 객체에 있는 함수를 쓸 수 있다.

    }
}