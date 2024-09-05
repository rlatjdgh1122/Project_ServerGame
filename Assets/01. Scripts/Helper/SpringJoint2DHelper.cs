using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint2D))]
public class SpringJoint2DHelper : ExpansionMonoBehaviour, ISpringJoint2DHandler, ISetupHandler
{
    private SpringJoint2D _joint = null;

    public void Setup(ComponentList list)
    {
        _joint = list.Find<SpringJoint2D>();
    }

    private void Start()
    {
        SetEnable(false);
    }

    public void SetTarget(Rigidbody2D target)
    {
        SetEnable(true);

        _joint.connectedBody = target;
    }

    public void SetDistance(float distance)
    {
        _joint.distance = distance;
    }

    public void SetDisable()
    {
        SetEnable(false);
    }
    private void SetEnable(bool value)
    {
        _joint.enabled = value;
    }
}
