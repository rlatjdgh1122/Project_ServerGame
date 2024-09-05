using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetSensor
{
    public IGrabTargetHandler FindTarget();
    public float GetDistance();
}
