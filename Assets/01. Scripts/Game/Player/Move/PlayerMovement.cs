using System;
using UnityEngine;

public class PlayerMovement : ExpansionMonoBehaviour, IMoveHandler
{
    public event Action OnMoveStoppedEvent;
    public event Action<Vector2, float> OnShootingEvent;

    private Vector2 _dir = Vector2.zero;
    private float _power = 0f;
       
    public void SetDirection(Vector2 dir)
    {
        _dir = dir;
    }

    public void SetPower(float power)
    {
        _power = power;
    }

    public void Shoot()
    {
        DoShoot(_dir, _power);
        OnShootingEvent?.Invoke(_dir, _power);
    }
    public void DoShoot(Vector2 dir, float power)
    {

    }

    public void MoveStop()
    {
        OnMoveStoppedEvent?.Invoke();
    }

   
}
