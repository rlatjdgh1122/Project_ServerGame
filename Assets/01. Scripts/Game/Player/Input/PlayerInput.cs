using Define;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : ExpansionMonoBehaviour, IPlayerInput, ISetupHandler
{

    private Dictionary<HASH_INPUT_PLAYER, Action<INPUT_KEY_STATE>> _inputEventDic = new();
    private Dictionary<HASH_INPUT_PLAYER, INPUT_KEY_STATE> _inputStateDic = new();
    private Dictionary<HASH_INPUT_PLAYER, Coroutine> _inputCoroutineDic = new();

    public void Setup(ComponentList list)
    {
        InputSetting();
    }

    public void InputSetting()
    {
        InputManager.Input.Player.SetCallbacks(this);
    }

    public void OnRegisterEvent(HASH_INPUT_PLAYER key, Action<INPUT_KEY_STATE> action)
    {

        if (!_inputEventDic.ContainsKey(key))
            _inputEventDic.Add(key, action);
        else
            _inputEventDic[key] += action;

        _inputStateDic.Add(key, INPUT_KEY_STATE.NOT_PUSHING);
    }

    public void RemoveRegisterEvent(HASH_INPUT_PLAYER key, Action<INPUT_KEY_STATE> action)
    {

        _inputEventDic[key] -= action;

        _inputStateDic.Remove(key);
    }


    public void OnGrapInput(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            _inputStateDic[HASH_INPUT_PLAYER.LeftClick] = INPUT_KEY_STATE.DOWN;
            _inputEventDic[HASH_INPUT_PLAYER.LeftClick].Invoke(INPUT_KEY_STATE.DOWN);

            Debug.Log("1");
            var corou = CoroutineUtil.CallWaitForActionUntilTrue(() => _inputStateDic[HASH_INPUT_PLAYER.LeftClick] == INPUT_KEY_STATE.UP, () =>
           {
               _inputStateDic[HASH_INPUT_PLAYER.LeftClick] = INPUT_KEY_STATE.PUSHING;
               _inputEventDic[HASH_INPUT_PLAYER.LeftClick].Invoke(INPUT_KEY_STATE.PUSHING);
               Debug.Log("2");

           });

            _inputCoroutineDic.Add(HASH_INPUT_PLAYER.LeftClick, corou);
        }

        if (context.canceled)
        {
            _inputStateDic[HASH_INPUT_PLAYER.LeftClick] = INPUT_KEY_STATE.UP;
            _inputEventDic[HASH_INPUT_PLAYER.LeftClick].Invoke(INPUT_KEY_STATE.UP);

            if (_inputCoroutineDic.TryGetValue(HASH_INPUT_PLAYER.LeftClick, out var corou))
            {
                Debug.Log("3");
                CoroutineUtil.StopCoroutine(corou);
                _inputCoroutineDic.Remove(HASH_INPUT_PLAYER.LeftClick);
            }
        }

    }

    public void OnStopInput(InputAction.CallbackContext context)
    {
        //스페이스바
        if (context.performed)
            Debug.Log("ClickA");
    }


}
