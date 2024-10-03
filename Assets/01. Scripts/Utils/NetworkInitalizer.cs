using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkInitalizer : ExpansionNetworkBehaviour
{
	private List<INetworkSpawnHandler> _interfaceList = new();

	public override void OnNetworkSpawn()
	{
		ComponentList list = new ComponentList(GetComponentsInChildren<Component>());

		_interfaceList = GetComponentsInChildren<INetworkSpawnHandler>().ToList();

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
