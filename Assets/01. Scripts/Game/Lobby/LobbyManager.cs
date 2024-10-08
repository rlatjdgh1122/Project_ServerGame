using ExtensionMethod.List;
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
	public Button ColorSelectButton = null;

	public UnityEvent OnSucceededGameStartEvent = null;
	public UnityEvent OnFailedGameStartEvent = null;

	public List<LobbyColorButton> ColorButtonList = new();

	public TurnType SelectTurnType = TurnType.None;

	private Dictionary<TurnType, LobbyColorButton> _colorToButtonDic = new();

	private void Awake()
	{
		ColorSelectButton.interactable = false;

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

	private void HandleSelectedeColor(LobbyColorButton button)
	{
		SelectTurnType = button.ColorType;
		ColorSelectButton.interactable = true;
		ColorButtonList.ObjExcept(button, btns => btns.OnDeSelected());
	}

	/// <summary>
	/// ���� Ȯ��, �ٽ� ���ٲ�
	/// </summary>
	public void OnColorConfirm()
	{
		//���������� �� ����
		ColorButtonList.ForEach(btn => btn.SetInteractable(true));
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
			button.OnConfirm();

			//���� ������ �ߴ� ���� ������� None���� ��ҽ�����
			if (data.turnType == SelectTurnType)
			{
				SelectTurnType = TurnType.None;
				ColorSelectButton.interactable = false;
			}
		}
	}

	/// <summary>
	/// �÷��̾ ������ �� ��ư�� �ٽ� ����
	/// </summary>
	private void OnRemoveDataEvent(UserData data)
	{
		if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
		{
			button.OnLeave();
		}
	}


	public void HandleGameStart()
	{
		if (CheckAllReady())
		{
			OnSucceededGameStartEvent?.Invoke();
		}
		else
		{
			foreach(var item in _notReadyUserList)
			{
				Debug.Log(item.playerName);
			}

			OnFailedGameStartEvent?.Invoke();
		}

	}

	private List<UserData> _notReadyUserList = new List<UserData>();

	/// <summary>
	/// ��� �÷��̾ �غ� �Ǿ����� Ȯ���ϴ� �Լ�
	/// </summary>
	private bool CheckAllReady()
	{
		_notReadyUserList.TryClear();

		bool result = true;

		foreach (UserData data in UserDataManager.Instance.UserDataList)
		{
			//�ѳ��̶� ���� ���þ��ߴٸ�
			if (data.turnType == TurnType.None)
			{
				_notReadyUserList.Add(data);
				result = false;
				break;
			}
		}

		return result;

	}
}
