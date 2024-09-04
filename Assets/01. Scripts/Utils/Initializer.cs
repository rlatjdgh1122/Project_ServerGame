using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : ExpansionMonoBehaviour
{

    public void Awake()
    {
       ComponentList list = new ComponentList(GetComponentsInChildren<Component>());

        foreach(var item in GetComponentsInChildren<ISetupHandler>())
        {
            item.Setup(list);
        }
    }
}
