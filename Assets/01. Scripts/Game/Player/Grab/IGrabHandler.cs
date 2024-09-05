using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabHandler
{
    public event Action OnStartGrabEvent;
    public event Action OnStopGrabEvent;

    public void DoGrab();
    public void Grab();

    public void Grabbing();
    public void DoGrabbing();

    public void DoGrabStop();
    public void GrabStop();
}
