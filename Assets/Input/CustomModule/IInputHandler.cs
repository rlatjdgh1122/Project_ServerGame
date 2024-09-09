using System;

namespace Seongho.InputSystem
{
    public interface IInputHandler<T> 
    {
        public void OnRegisterEvent(T key, InputParams action);
        public void RemoveRegisterEvent(T key, InputParams action);
    }
    public interface IPlayerInput : IInputHandler<HASH_INPUT_PLAYER>, PlayerAction.IPlayerActions
    {
        public void InputSetting();
    }

    public interface IUIInput : IInputHandler<HASH_INPUT_UI>, PlayerAction.IUIActions
    {
        public void InputSetting();
    }
}



