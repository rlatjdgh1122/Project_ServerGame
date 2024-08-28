using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapObject : ExpansionMonoBehaviour, IGrapHandler
{
    public event Action OnStartGrapEvent;
    public event Action OnStopGrapEvent;

    private SpringJoint2D _sj;

    private void Awake()
    {
        _sj = GetComponent<SpringJoint2D>();
    }
    public void Grap()
    {

    }

    public void GrapStop()
    {

    }


    public void DoGrap()
    {

    }

    public void DoGrapStop()
    {

    }
}
