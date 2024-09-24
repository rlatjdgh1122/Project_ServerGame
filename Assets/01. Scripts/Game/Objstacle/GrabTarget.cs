using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class GrabTarget : ExpansionMonoBehaviour, IGrabTargetHandler
{
    private Rigidbody2D _rb = null;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public Rigidbody2D GetRigidBody()
    {
        return _rb;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
