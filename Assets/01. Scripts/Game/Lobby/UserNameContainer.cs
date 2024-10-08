using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserNameContainer : ExpansionMonoBehaviour
{
	public UserNameText UserNamePrefab = null;

	private Dictionary<ulong, UserNameText> _clientIDToTextDic = new();

	private void Awake()
	{
		UserDataManager.Instance.OnAddUserEvent += OnAddUserEvent;
		UserDataManager.Instance.OnValueChangedUserEvent += OnChangedUserEvent;
		UserDataManager.Instance.OnRemoveUserEvent += OnRemoveUserEvent;
	}


	private void OnAddUserEvent(UserData data)
	{
		var prefab = Instantiate(UserNamePrefab, transform);
		prefab.SetName(data.playerName.ToString());
		prefab.SetColor(data.turnType);

		_clientIDToTextDic.Add(data.clientId, prefab);
	}

	private void OnChangedUserEvent(UserData data)
	{
		var text = _clientIDToTextDic[data.clientId];
		text.SetColor(data.turnType);

		_clientIDToTextDic[data.clientId] = text;
	}

	private void OnRemoveUserEvent(UserData data)
	{
		_clientIDToTextDic.Remove(data.clientId);
	}
}
