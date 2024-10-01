using Seongho.InputSystem;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class PlayerInput : ExpansionNetworkBehaviour, IPlayerInput
{
    private InputMachine<HASH_INPUT_PLAYER> _inputContainer = null;

	public override void OnNetworkSpawn()
	{

		if(!IsOwner)
			InputManager.Input.Player.Disable();

		if (!IsOwner) return;
		InputManager.CreateMachine(out _inputContainer);

		InputSetting();
	}

	public void InputSetting()
    {
        InputManager.Input.Player.SetCallbacks(this);
    }

    public void OnRegisterEvent(HASH_INPUT_PLAYER key, InputParams action)
    {
		Debug_S.Log(IsOwner);
		if (!IsOwner) return;

        _inputContainer.OnRegisterEvent(key, action);
    }

    public void RemoveRegisterEvent(HASH_INPUT_PLAYER key, InputParams action)
    {
		Debug_S.Log(IsOwner);
		if (!IsOwner) return;

		_inputContainer.RemoveRegisterEvent(key, action);
    }

    public void OnLeftClickInput(InputAction.CallbackContext context)
	{
		Debug_S.Log(IsOwner);
		if (!IsOwner) return;

		_inputContainer.InputRunning(HASH_INPUT_PLAYER.LeftClick, context, true);
    }

    public void OnSpaceClickInput(InputAction.CallbackContext context)
	{
		Debug_S.Log(IsOwner);
		if (!IsOwner) return;

		_inputContainer.InputRunning(HASH_INPUT_PLAYER.Space, context, false);
    }
}
