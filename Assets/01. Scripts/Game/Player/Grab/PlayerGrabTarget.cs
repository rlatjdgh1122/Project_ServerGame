using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabTarget : GrabTarget, ITurnChangedHandler
{
    private CircleCollider2D _coll = null;

    protected override void Awake()
    {
        base.Awake();

        _coll = GetComponent<CircleCollider2D>();
    }

    void ITurnChangedHandler.OnTurnChanged(TurnType prevType, TurnType newType)
    {
        if (TurnManager.Instance.IsMyTurn)
        {
            _coll.enabled = false;

        } //end if

        else
        {
            _coll.enabled = true;

        } //end else
    }

}
