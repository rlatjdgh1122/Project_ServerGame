using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DHelper : ExpansionMonoBehaviour, IRigidbody2DHandler
{
    private Rigidbody2D _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public Vector2 GetVelocity() => _rb.velocity;
    public float GetAngularVelocity() => _rb.angularVelocity;

    public void SetGravity(float value) => _rb.gravityScale = value;
    public void SetVelocity(Vector2 value) => _rb.velocity = value;
    public void SetAngularVelocity(float value) => _rb.angularVelocity = value;

}
