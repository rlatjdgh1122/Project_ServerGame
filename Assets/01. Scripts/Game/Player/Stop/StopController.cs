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
        //�����̶�� Start���ִ°� ó��
        _inputHandler.OnRegisterEvent(_stopKey, KeyEvent_OnStop);
    }

    private void RemoveRegister()
    {
        _inputHandler.RemoveRegisterEvent(_stopKey, KeyEvent_OnStop);
    }

    //������ �ƴ϶�� ��� ��ž���ش�.
    void ITurnChangedHandler.OnNotMyTurn()
    {
        OnStop();
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
            _TT = !_TT;

            //ó�� �����Ŷ�� �÷��̾��� ���¸� �������ش�.
            if (_TT)
            {
                OnStart();

            } //end if

            //�ι�°�� �����Ŷ�� �÷��̾��� ���¸� ���� �� ���� �ѱ��.
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
