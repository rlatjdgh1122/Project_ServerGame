namespace Seongho.InputSystem
{
    public interface IInputHandler<T> 
    {
        public void OnRegisterEvent(T key, InputParams action);
        public void RemoveRegisterEvent(T key, InputParams action);
    }

    public interface IMobileInput : IInputHandler<HASH_INPUT_MOBILE>, PlayerAction.IMobileActions
    {
        public void InputSetting();
    }

    public interface IUIInput : IInputHandler<HASH_INPUT_UI>, PlayerAction.IUIActions
    {
        public void InputSetting();
    }
}



