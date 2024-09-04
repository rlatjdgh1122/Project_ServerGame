using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint2D))]
public class SpringJoint2DHelper : ExpansionMonoBehaviour, ISpringJoint2DHandler
{
    private SpringJoint2D _sj = null;

    private void Awake()
    {
        _sj = GetComponent<SpringJoint2D>();
    }

    public void SetTarget(Rigidbody2D target)
    {
        _sj.connectedBody = target;
    }
}
