using Seongho.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopController : ExpansionMonoBehaviour, ISetupHandler
{
    private List<IPlayerStopHandler> _stopHandlerList = new();

    private IInputHandler<HASH_INPUT_PLAYER> _inputHandler = null;

    public void Setup(ComponentList list)
    {
        _stopHandlerList = list.FindAll<IPlayerStopHandler>();
        _inputHandler = list.Find<IInputHandler<HASH_INPUT_PLAYER>>();

        OnRegister();
    }

    private void OnRegister()
    {
        //�����̶�� Start���ִ°� ó��
        _inputHandler.OnRegisterEvent(HASH_INPUT_PLAYER.Space, OnStop);
    }

    private void RemoveRegister()
    {
        _inputHandler.RemoveRegisterEvent(HASH_INPUT_PLAYER.Space, OnStop);
    }

    public void OnStart(INPUT_KEY_STATE key, params object[] args)
    {
        if (key == INPUT_KEY_STATE.DOWN)
        {
            foreach (var item in _stopHandlerList)
            {
                item.OnPlayerStart();
            }

        } //end keyDown

    }

    /// <summary>
    /// Space�ٸ� ���� ��� ����˴ϴ�.
    /// </summary>
    public void OnStop(INPUT_KEY_STATE key, params object[] args)
    {
        if (key == INPUT_KEY_STATE.DOWN)
        {
            foreach (var item in _stopHandlerList)
            {
                item.OnPlayerStop();
            }

        } //end keyDown
    }
    private void OnDestroy()
    {
        RemoveRegister();
    }
}
