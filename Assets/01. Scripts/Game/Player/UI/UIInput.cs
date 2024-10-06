using Define;
using Seongho.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : ExpansionMonoBehaviour, IUIInput, ISetupHandler
{
    private InputMachine<HASH_INPUT_UI> _inputMachine = null;

    public void Setup(ComponentList list)
    {
        InputManager.CreateMachine(out _inputMachine);
        InputSetting();
    }

    public void InputSetting()
    {
        InputManager.Input.UI.SetCallbacks(this);
    }

    public void OnRegisterEvent(HASH_INPUT_UI key, InputParams action)
    {
        
    }

    public void RemoveRegisterEvent(HASH_INPUT_UI key, InputParams action)
    {
        
    }


    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("UI");
    }

  
}
