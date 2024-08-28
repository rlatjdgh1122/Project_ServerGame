using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrapHandler
{
    public event Action OnStartGrapEvent;
    public event Action OnStopGrapEvent;

    public void DoGrap();
    public void Grap();
    public void DoGrapStop();
    public void GrapStop();
}
