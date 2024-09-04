using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GrapObject : ExpansionMonoBehaviour, IGrapHandler
{
    [SerializeField] private SpringJoint2D _springJoint;

    public event Action OnStartGrapEvent;
    public event Action OnStopGrapEvent;

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
