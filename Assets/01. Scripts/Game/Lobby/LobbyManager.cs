using ExtensionMethod.Dictionary;
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
	private List<UserData> _userDataList = new List<UserData>();

	private void Awake()
	{
		ColorSelectButton.interactable = false;

		foreach (var button in ColorButtonList)
		{
			button.OnClickEvent += HandleSelectedeColor;
			_colorToButtonDic.Add(button.ColorType, button);

		} //end foreach
		UserDataManager.Instance.OnAddUserEvent += OnAddUserEvent;
		UserDataManager.Instance.OnValueChangedUserEvent += OnChangedDataEvent;
		UserDataManager.Instance.OnRemoveUserEvent += OnRemoveDataEvent;
	}

	private void OnDestroy()
	{
		foreach (var button in ColorButtonList)
		{
			button.OnClickEvent -= HandleSelectedeColor;

		} //end foreach

		_colorToButtonDic.TryClear();
		_userDataList.TryClear();

		UserDataManager.Instance.OnAddUserEvent -= OnAddUserEvent;
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
		ColorButtonList.ForEach(btn => btn.SetInteractable(false));
		UserDataManager.Instance.ColorConfirm(SelectTurnType);
	}

	/// <summary>
	/// ������ �߰��� �� ����
	/// ������ �ִ� ������ ������ �߰���
	/// </summary>
	private void OnAddUserEvent(UserData data)
	{
		_userDataList.Add(data);
	}

	/// <summary>
	/// �����Ͱ� ����� ��
	/// ������ ���÷��� �������� �� �����
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

			} //end if
		} //end if
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

	public void OnGameStart()
	{
		if (CheckAllReady())
		{
			OnSucceededGameStartEvent?.Invoke();

		} //end if
		else
		{
			OnFailedGameStartEvent?.Invoke();

		} //end else

	}

	/// <summary>
	/// ��� �÷��̾ �غ� �Ǿ����� Ȯ���ϴ� �Լ�
	/// </summary>
	private bool CheckAllReady()
	{
		//��� Ŭ���̾�Ʈ�� ���� �ؾ���
		return _userDataList.TrueForAll(data => data.turnType != TurnType.None);
	}

}
