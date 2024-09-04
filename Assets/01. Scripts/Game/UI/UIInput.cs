using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : ExpansionMonoBehaviour, IUIInput, ISetupHandler
{

    private Dictionary<HASH_INPUT_UI, Action<INPUT_KEY_STATE>> _inputEventDic = new();

    public void Setup(ComponentList list)
    {
        InputSetting();
    }

    public void InputSetting()
    {
        InputManager.Input.UI.SetCallbacks(this);
    }

    public void OnRegisterEvent(HASH_INPUT_UI key, Action<INPUT_KEY_STATE> action)
    {
        
    }

    public void RemoveRegisterEvent(HASH_INPUT_UI key, Action<INPUT_KEY_STATE> action)
    {
        
    }


    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            Debug.Log("UI");
    }

  
}
