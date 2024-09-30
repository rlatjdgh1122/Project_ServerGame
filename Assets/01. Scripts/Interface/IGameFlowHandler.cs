using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFlowHandler : IInterfaceSetUpHandler
{
	void IInterfaceSetUpHandler.IStart()
	{
		SignalHub.OnGameStartEvent += OnGameStart;
		SignalHub.OnGameEndEvent += OnGameEnd;
	}

	void IInterfaceSetUpHandler.IDestroy()
	{
		SignalHub.OnGameStartEvent -= OnGameStart;
		SignalHub.OnGameEndEvent -= OnGameEnd;
	}

	public void OnGameStart() { }
	public void OnGameEnd() { }
}
