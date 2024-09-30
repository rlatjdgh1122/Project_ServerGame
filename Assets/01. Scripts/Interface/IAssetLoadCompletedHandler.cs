using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetLoadCompletedHandler : IInterfaceSetUpHandler
{
	void IInterfaceSetUpHandler.IStart()
	{
		SignalHub.OnAssetLoadCompetedEvent += OnSuccessedLoadAsset;
	}

	void IInterfaceSetUpHandler.IDestroy()
	{
		SignalHub.OnAssetLoadCompetedEvent -= OnSuccessedLoadAsset;
	}

	public void OnSuccessedLoadAsset();
}
