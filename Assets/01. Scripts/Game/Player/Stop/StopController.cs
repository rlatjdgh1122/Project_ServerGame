using Seongho.InputSystem;
using System.Collections.Generic;
using UnityEngine;

public class StopController : ExpansionMonoBehaviour, ISetupHandler, INetworkSpawnHandler, ITurnChangedHandler
{
    [SerializeField] private HASH_INPUT_PLAYER _stopKey = HASH_INPUT_PLAYER.Space;

    private List<IPlayerStopHandler> _stopHandlerList = new();

    private IInputHandler<HASH_INPUT_PLAYER> _inputHandler = null;

    private bool _TT = false;

    public void Setup(ComponentList list)
    {
        _stopHandlerList = list.FindAll<IPlayerStopHandler>();
        _inputHandler = list.Find<IInputHandler<HASH_INPUT_PLAYER>>();
    }

    public void OnSpawn()
    {
        OnRegister();
    }

    public void OnDespawn() { }

    private void OnRegister()
    {
        //내턴이라면 Start해주는거 처리
        _inputHandler.OnRegisterEvent(_stopKey, KeyEvent_OnStop);
    }

    private void RemoveRegister()
    {
        _inputHandler.RemoveRegisterEvent(_stopKey, KeyEvent_OnStop);
    }

    //내턴이 아니라면 계속 스탑해준다.
    void ITurnChangedHandler.OnNotMyTurn()
    {
        OnStop();
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
            _TT = !_TT;

            //처음 누른거라면 플레이어의 상태를 시작해준다.
            if (_TT)
            {
                OnStart();

            } //end if

            //두번째로 누른거라면 플레이어의 상태를 멈춘 후 턴을 넘긴다.
            else
            {
                OnStop();

                TurnManager.Instance.OnTurnChangedNext();
            } //end else

        } //end keyDown
    }

    private void OnStart()
    {
        foreach (var item in _stopHandlerList)
        {
            item.OnPlayerStart();
        }

    } //end keyDown

    private void OnStop()
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
