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
	/// 선택 확정, 다시 못바꿈
	/// </summary>
	public void OnColorConfirm()
	{
		//선택했으면 다 꺼줌
		ColorButtonList.ForEach(btn => btn.SetInteractable(true));
		UserDataManager.Instance.ColorConfirm(SelectTurnType);
	}

	/// <summary>
	/// 데이터가 변경될 때
	/// ex) TurnType이 변경되면 모든 클라에서 버튼의 interactable을 꺼줌
	/// </summary>
	/// <param name="data"></param>
	private void OnChangedDataEvent(UserData data)
	{
		if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
		{
			button.OnConfirm();

			//내가 고를려고 했던 것을 골랐을때 None으로 취소시켜줌
			if (data.turnType == SelectTurnType)
			{
				SelectTurnType = TurnType.None;
				ColorSelectButton.interactable = false;
			}
		}
	}

	/// <summary>
	/// 플레이어가 나가면 그 버튼은 다시 켜줌
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
	/// 모든 플레이어가 준비가 되었는지 확인하는 함수
	/// </summary>
	private bool CheckAllReady()
	{
		_notReadyUserList.TryClear();

		bool result = true;

		foreach (UserData data in UserDataManager.Instance.UserDataList)
		{
			//한놈이라도 턴을 선택안했다면
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
