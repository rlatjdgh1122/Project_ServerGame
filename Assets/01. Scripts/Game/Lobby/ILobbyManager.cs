using System;

public interface ILobbyManager : IUserDataEventHandler
{
	void OnColorConfirm();
	void OnGameStart();
}