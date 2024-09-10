using Seongho.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopController : ExpansionMonoBehaviour, ISetupHandler
{
    private List<IPlayerStopHandler> _stopHandlerList = new();

    private IInputHandler<HASH_INPUT_PLAYER> _inputHandler = null;

    private bool Test = false;
    public void Setup(ComponentList list)
    {
        _stopHandlerList = list.FindAll<IPlayerStopHandler>();
        _inputHandler = list.Find<IInputHandler<HASH_INPUT_PLAYER>>();

        OnRegister();
    }

    private void OnRegister()
    {
        //내턴이라면 Start해주는거 처리
        _inputHandler.OnRegisterEvent(HASH_INPUT_PLAYER.Space, OnStop);
    }

    private void RemoveRegister()
    {
        _inputHandler.RemoveRegisterEvent(HASH_INPUT_PLAYER.Space, OnStop);
    }

    public void OnStart()
    {
        foreach (var item in _stopHandlerList)
        {
            item.OnPlayerStart();
        }

    } //end keyDown


    /// <summary>
    /// Space바를 누를 경우 실행됩니다.
    /// </summary>
    public void OnStop(INPUT_KEY_STATE key, params object[] args)
    {
        if (key == INPUT_KEY_STATE.DOWN)
        {
            Test = !Test;

            if (Test)
            {
                foreach (var item in _stopHandlerList)
                {
                    item.OnPlayerStop();
                }
            }
            else
            {
                foreach (var item in _stopHandlerList)
                {
                    item.OnPlayerStart();
                }
            }


        } //end keyDown
    }
    private void OnDestroy()
    {
        RemoveRegister();
    }
}
