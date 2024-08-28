using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArrow : ExpansionMonoBehaviour, IArrowHandler
{
    [SerializeField] private Transform _arrowHandlerTrm = null;
    [SerializeField] private float _rotateSpeed = 1f;

    public event Action OnStartRotateEvent;
    public event Action OnStopRotateEvent;

    public void DoRatateStop()
    {
        throw new NotImplementedException();
    }

    public void DoRotate()
    {
        throw new NotImplementedException();
    }

    public void Rotate()
    {
        throw new NotImplementedException();
    }

    public void RotateStop()
    {
        throw new NotImplementedException();
    }
}
