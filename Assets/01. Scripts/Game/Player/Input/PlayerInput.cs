using Define;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class PlayerInput : ExpansionMonoBehaviour, IPlayerInput, ISetupHandler
{

    private InputMachine<HASH_INPUT_PLAYER> _inputContainer = null;

    public void Setup(ComponentList list)
    {
        InputManager.CreateMachine(out _inputContainer);
        InputSetting();
    }

    public void InputSetting()
    {
        InputManager.Input.Player.SetCallbacks(this);
    }

    public void OnRegisterEvent(HASH_INPUT_PLAYER key, InputParams action)
    {

        _inputContainer.OnRegisterEvent(key, action);
    }

    public void RemoveRegisterEvent(HASH_INPUT_PLAYER key, InputParams action)
    {

        _inputContainer.RemoveRegisterEvent(key, action);

    }

    public void OnLeftClickInput(InputAction.CallbackContext context)
    {

        _inputContainer.InputRunning(HASH_INPUT_PLAYER.LeftClick, context, true);

    }

    public void OnSpaceClickInput(InputAction.CallbackContext context)
    {

        _inputContainer.InputRunning(HASH_INPUT_PLAYER.Space, context, false);

    }
}
