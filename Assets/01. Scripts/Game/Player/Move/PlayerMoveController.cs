using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler
{
    private IRigidbody2DHandler _rigid = null;

    private Vector2 _saveVelocity = Vector2.zero;

    public void Setup(ComponentList list)
    {
        _rigid = list.Find<IRigidbody2DHandler>();
    }

    public void OnPlayerStart()
    {
        _rigid.SetGravity(1);

        _rigid.SetVelocity(_saveVelocity);
    }

    public void OnPlayerStop()
    {
        _rigid.SetGravity(0);

        _saveVelocity = _rigid.GetVelocity();
        _rigid.SetVelocity(Vector2.zero);
    }

}
