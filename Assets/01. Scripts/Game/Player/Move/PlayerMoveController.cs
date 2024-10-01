using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler
{
    private IRigidbody2DHandler _rigid = null;
    private IDrawMovePathHandler _draw = null;

    private Vector2 _saveVelocity = Vector2.zero;
    private float _angularVelocity = 0f;

    public void Setup(ComponentList list)
    {
        _rigid = list.Find<IRigidbody2DHandler>();
        _draw = list.Find<IDrawMovePathHandler>();
    }

    public void OnPlayerStart()
    {
        _rigid.SetGravity(1);
        _rigid.SetVelocity(_saveVelocity);
        _rigid.SetAngularVelocity(_angularVelocity);

        _draw.Clear();
    }

    public void OnPlayerStop()
    {
        _rigid.SetGravity(0);
        _saveVelocity = _rigid.GetVelocity();
        _angularVelocity = _rigid.GetAngularVelocity();

        _rigid.SetVelocity(Vector2.zero);
        _rigid.SetAngularVelocity(0);

        _draw.OnDraw(_saveVelocity);
    }

}
