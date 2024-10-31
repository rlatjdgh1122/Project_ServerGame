using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameFlowHandler : IInterfaceSetUpHandler
{
	void IInterfaceSetUpHandler.IStart()
	{
		SignalHub.OnGameInitEvent += OnGameInit;
		SignalHub.OnGameStartEvent += OnGameStart;
		SignalHub.OnGameEndEvent += OnGameEnd;
	}

	void IInterfaceSetUpHandler.IDestroy()
	{
		SignalHub.OnGameInitEvent -= OnGameInit;
		SignalHub.OnGameStartEvent -= OnGameStart;
		SignalHub.OnGameEndEvent -= OnGameEnd;
	}

	public void OnGameInit() { }
    public void OnGameStart() { }
	public void OnGameEnd() { }
}
