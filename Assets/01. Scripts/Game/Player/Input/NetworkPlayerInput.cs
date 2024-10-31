using Seongho.InputSystem;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class NetworkPlayerInput : ExpansionNetworkBehaviour, IMobileInput, INetworkSpawnHandler
{
    private InputMachine<HASH_INPUT_MOBILE> _inputContainer = null;

    public void OnSpawn()
    {
        if (!IsOwner) return;

        InputManager.CreateMachine(out _inputContainer);
        InputSetting();
    }

    public void OnDespawn()
    {

    }

    public void InputSetting()
    {
        InputManager.Input.Mobile.SetCallbacks(this);
    }

    public void OnRegisterEvent(HASH_INPUT_MOBILE key, InputParams action)
    {
        if (!IsOwner) return;

        _inputContainer.OnRegisterEvent(key, action);
    }

    public void RemoveRegisterEvent(HASH_INPUT_MOBILE key, InputParams action)
    {
        if (!IsOwner) return;

        _inputContainer.RemoveRegisterEvent(key, action);
    }

    public void OnTouchInput(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        _inputContainer.InputRunning(HASH_INPUT_MOBILE.Touch, context, true);
    }

    public void OnDoubleTouchInput(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        _inputContainer.InputRunning(HASH_INPUT_MOBILE.Double_Touch, context, false);
    }
}
