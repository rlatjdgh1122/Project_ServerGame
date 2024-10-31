using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ExpansionNetworkBehaviour, ISetupHandler
{

    public void Setup(ComponentList list)
    {
       
    }

    public void SetPostion(Transform trm)
    {
        transform.position = trm.position;
    }
}
