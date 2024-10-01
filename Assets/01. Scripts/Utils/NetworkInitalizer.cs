using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkInitalizer : ExpansionNetworkBehaviour
{
	private List<IInterfaceNetworkHandler> _interfaceList = new();

	public override void OnNetworkSpawn()
	{
		ComponentList list = new ComponentList();

		_interfaceList = GetComponentsInChildren<IInterfaceNetworkHandler>().ToList();

		foreach (var item in _interfaceList)
		{
			item.OnSpawn();

		} //end foreach
	}

	public override void OnNetworkDespawn()
	{
		foreach (var item in _interfaceList)
		{
			item.OnDespawn();

		} //end foreach
	}
}
