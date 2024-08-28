using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArrowHandler
{
    public event Action OnStartRotateEvent;
    public event Action OnStopRotateEvent;

    public void DoRotate();
    public void Rotate();
    public void DoRatateStop();
    public void RotateStop();
}
