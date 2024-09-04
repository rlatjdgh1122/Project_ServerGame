using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetController : ExpansionMonoBehaviour, ISetupHandler
{

    private IInputHandler<HASH_INPUT_PLAYER> _inputHandler = null; 

    public void Setup(ComponentList list)
    {
        _inputHandler = list.Find<IInputHandler<HASH_INPUT_PLAYER>>();

        _inputHandler.OnRegisterEvent(HASH_INPUT_PLAYER.LeftClick, FindTarget);
    }

    private void FindTarget(INPUT_KEY_STATE state)
    {

    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.RemoveRegisterEvent(HASH_INPUT_PLAYER.LeftClick, FindTarget);
        }
    }

}
