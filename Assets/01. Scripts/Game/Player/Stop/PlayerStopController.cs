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
        //�����̶�� Start���ִ°� ó��
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
        Debug.Log("�� ���ʱ�");
    }


    /// <summary>
    /// Space�ٸ� ���� ��� ����˴ϴ�.
    /// </summary>
    public void KeyEvent_OnStop(INPUT_KEY_STATE key, params object[] args)
    {
        if (!TurnManager.Instance.IsMyTurn) return;

        //�� ���� ���¿��� Ű�� ������ �ȴٸ�
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
