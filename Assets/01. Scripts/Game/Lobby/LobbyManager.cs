using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyManager : ExpansionMonoBehaviour, ILobbyManager
{
	public Button GameStartButton = null;

	public UnityEvent OnSucceededGameStartEvent = null;
	public UnityEvent OnFailedGameStartEvent = null;

	public List<LobbyColorButton> ColorButtonList = new();

	public TurnType SelectTurnType = TurnType.None;

	private Dictionary<TurnType, LobbyColorButton> _colorToButtonDic = new();

	private void Awake()
	{
		foreach (var button in ColorButtonList)
		{
			button.OnClickEvent += HandleSelectedeColor;
			_colorToButtonDic.Add(button.ColorType, button);

		} //end foreach

		UserDataManager.Instance.OnValueChangedUserEvent += OnChangedDataEvent;
		UserDataManager.Instance.OnRemoveUserEvent += OnRemoveDataEvent;
	}

	private void OnDestroy()
	{
		foreach (var button in ColorButtonList)
		{
			button.OnClickEvent -= HandleSelectedeColor;

		} //end foreach

		_colorToButtonDic.Clear();

		UserDataManager.Instance.OnValueChangedUserEvent -= OnChangedDataEvent;
		UserDataManager.Instance.OnRemoveUserEvent -= OnRemoveDataEvent;

	}

	private void HandleSelectedeColor(TurnType type)
	{
		SelectTurnType = type;
	}

	/// <summary>
	/// ���� Ȯ��, �ٽ� ���ٲ�
	/// </summary>
	public void OnColorConfirm()
	{
		UserDataManager.Instance.ColorConfirm(SelectTurnType);
	}

	/// <summary>
	/// �����Ͱ� ����� ��
	/// ex) TurnType�� ����Ǹ� ��� Ŭ�󿡼� ��ư�� interactable�� ����
	/// </summary>
	/// <param name="data"></param>
	private void OnChangedDataEvent(UserData data)
	{
		if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
		{
			button.Button.interactable = false;
		}
	}

	/// <summary>
	/// �÷��̾ ������ �� ��ư�� �ٽ� ����
	/// </summary>
	private void OnRemoveDataEvent(UserData data)
	{
		if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
		{
			button.Button.interactable = true;
		}
	}


	public void HandleGameStart()
	{

	}
}
