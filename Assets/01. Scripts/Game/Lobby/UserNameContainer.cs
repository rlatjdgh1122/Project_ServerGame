using ExtensionMethod.Dictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UserNameContainer : ExpansionMonoBehaviour
{
	[SerializeField] private UserNameText _userNamePrefab = null;
	[SerializeField] private Transform _containerTrim = null;

	private Dictionary<ulong, UserNameText> _clientIDToTextDic = new();

	public void CreateUserData(UserData data)
	{
		UserNameText prefab = Instantiate(_userNamePrefab, _containerTrim);

		prefab.SetName(data.playerName.ToString());
		prefab.SetColor(data.turnType);

		_clientIDToTextDic.Add(data.clientId, prefab);
	}

	public void ChangedUserData(UserData data)
	{
		if (_clientIDToTextDic.TryGetValue(data.clientId, out var result))
		{
			result.SetColor(data.turnType);
			_clientIDToTextDic[data.clientId] = result;

		} //end if
	}

	public void RemoveUserData(UserData data)
	{
		_clientIDToTextDic.TryRemove(data.clientId, text => Destroy(text.gameObject));
	}
}
