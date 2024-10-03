using System;

public interface ILocalActionHandler<T> where T : Enum
{
    public event Action<T> OnActionEvent;

    public void DoAction(T type);
}