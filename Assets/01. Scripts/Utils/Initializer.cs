using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Initializer : ExpansionMonoBehaviour
{
    private List<IInterfaceSetUpHandler> _interfaceList = new();

    public void Awake()
    {
        ComponentList list = new ComponentList(GetComponentsInChildren<Component>());
        _interfaceList = GetComponentsInChildren<IInterfaceSetUpHandler>().ToList();

        foreach (var item in GetComponentsInChildren<ISetupHandler>())
        {
            item.Setup(list);

        } //end foreach

        foreach (var item in _interfaceList)
        {
            item.IStart();

        } //end foreach
    }

    private void OnDestroy()
    {
        foreach (var item in _interfaceList)
        {
            item.IDestroy();

        } //end foreach
    }

}
