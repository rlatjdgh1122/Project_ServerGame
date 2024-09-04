using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInputHandler<T> where T : Enum
{
    public void OnRegisterEvent(T key, Action<INPUT_KEY_STATE> action);
    public void RemoveRegisterEvent(T key, Action<INPUT_KEY_STATE> action);
}

public interface IPlayerInput : IInputHandler<HASH_INPUT_PLAYER>, PlayerAction.IPlayerActions
{
    public void InputSetting();
}

public interface IUIInput : IInputHandler<HASH_INPUT_UI>, PlayerAction.IUIActions
{
    public void InputSetting();
}
