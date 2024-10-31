using Seongho.InputSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopController : ExpansionMonoBehaviour, ISetupHandler, INetworkSpawnHandler, ITurnChangedHandler, IPlayerStopController
{
    [SerializeField] private HASH_INPUT_MOBILE _stopKey = HASH_INPUT_MOBILE.Double_Touch;
    [SerializeField] private HASH_INPUT_MOBILE _startKey = HASH_INPUT_MOBILE.Touch;

    private List<IPlayerStopHandler> _stopHandlerList = new();

    private IInputHandler<HASH_INPUT_MOBILE> _inputHandler = null;

    private bool _TT = false;

    public void Setup(ComponentList list)
    {
        _stopHandlerList = list.FindAll<IPlayerStopHandler>();
        _inputHandler = list.Find<IInputHandler<HASH_INPUT_MOBILE>>();
    }

    public void OnSpawn()
    {
        OnRegister();
    }

    public void OnDespawn() { }

    private void OnRegister()
    {
        //내턴이라면 Start해주는거 처리
        _inputHandler.OnRegisterEvent(_startKey, KeyEvent_OnStart);
        _inputHandler.OnRegisterEvent(_stopKey, KeyEvent_OnStop);
    }

    private void RemoveRegister()
    {
        _inputHandler.RemoveRegisterEvent(_startKey, KeyEvent_OnStart);
        _inputHandler.RemoveRegisterEvent(_stopKey, KeyEvent_OnStop);
    }

    private void KeyEvent_OnStart(INPUT_KEY_STATE key, object[] args)
    {

    }

    void ITurnChangedHandler.OnMyTurn()
    {
        Debug.Log("내 차례군");
    }


    /// <summary>
    /// Space바를 누를 경우 실행됩니다.
    /// </summary>
    public void KeyEvent_OnStop(INPUT_KEY_STATE key, params object[] args)
    {
        if (!TurnManager.Instance.IsMyTurn) return;

        //내 턴인 상태에서 키를 누르게 된다면
        if (key == INPUT_KEY_STATE.DOWN)
        {
            OnStop();
            TurnManager.Instance.OnTurnChangedNext();

        } //end keyDown
    }

    public void OnStart()
    {
        foreach (var item in _stopHandlerList)
        {
            item.OnPlayerStart();
        }

    } //end keyDown

    public void OnStop()
    {
        foreach (var item in _stopHandlerList)
        {
            item.OnPlayerStop();
        }
    }


    private void OnDestroy()
    {
        RemoveRegister();
    }
}
