using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveHandler
{
    public event Action OnMoveStoppedEvent;
    public event Action<Vector2, float> OnShootingEvent;

    public void SetDirection(Vector2 dir);
    public void SetPower(float power);
    public void MoveStop();
    public void DoShoot(Vector2 dir, float power);
    public void Shoot();
}
