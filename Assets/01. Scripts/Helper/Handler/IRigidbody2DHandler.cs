using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRigidbody2DHandler
{
    public Vector2 GetVelocity();
    public float GetAngularVelocity();
    public void SetGravity(float value);
    public void SetVelocity(Vector2 value);
    public void SetAngularVelocity(float value);
}
