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
	/// 선택 확정, 다시 못바꿈
	/// </summary>
	public void OnColorConfirm()
	{
		//선택했으면 다 꺼줌
		ColorButtonList.ForEach(btn => btn.SetInteractable(false));
		UserDataManager.Instance.ColorConfirm(SelectTurnType);
	}

	/// <summary>
	/// 유저가 추가될 때 실행
	/// 기존에 있던 유저의 정보도 추가됨
	/// </summary>
	private void OnAddUserEvent(UserData data)
	{
		_userDataList.Add(data);
	}

	/// <summary>
	/// 데이터가 변경될 때
	/// 유저가 팀컬러를 선택했을 때 실행됨
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

			} //end if
		} //end if
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
	/// 모든 플레이어가 준비가 되었는지 확인하는 함수
	/// </summary>
	private bool CheckAllReady()
	{
		//모든 클라이언트가 레디를 해야함
		return _userDataList.TrueForAll(data => data.turnType != TurnType.None);
	}

}
