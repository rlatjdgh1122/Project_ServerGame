using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class DistanceJoint2DHelper : ExpansionMonoBehaviour, IDistanceJoint2DHandler, ISetupHandler
{
    private DistanceJoint2D _joint = null;

    public void Setup(ComponentList list)
    {
        _joint = list.Find<DistanceJoint2D>();
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
