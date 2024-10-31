using ExtensionMethod.Dictionary;
using ExtensionMethod.List;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 경쟁모드 로비 UI
/// </summary>
public class CompetitionLobbyUI : ExpansionNetworkBehaviour, ILobbyManager
{
    public UserNameContainer NameContainer = null;

    public UserDataController DataController = null;

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
        DataController.OnResiter(this);

        ColorSelectButton.interactable = false;

        foreach (var button in ColorButtonList)
        {
            button.OnClickEvent += HandleSelectedeColor;
            _colorToButtonDic.Add(button.ColorType, button);

        } //end foreach
    }

    private void Start()
    {
        //UserDataController.Instance.OnResiter(this);
    }

    /*public override void OnNetworkSpawn()
	{

		if (IsClient)
		{
			GameStartButton.gameObject.SetActive(false);
		}

		if (IsServer)
		{
			GameStartButton.gameObject.SetActive(true);
		}
	}*/


    public void OnGameStart()
    {
        if (CheckAllReady())
        {
            OnSucceddedEventHanlderServerRpc();

        } //end if
        else
        {
            OnFailedEventHanlderServerRpc();

        } //end else
    }

    [ServerRpc]
    private void OnSucceddedEventHanlderServerRpc()
    {
        if (!IsHost)
        {
            Debug.Log("클라는 시작할 수 없음");
        }
        OnSucceddedEventHanlderClientRpc();
    }

    [ClientRpc]
    private void OnSucceddedEventHanlderClientRpc()
    {
        OnSucceededGameStartEvent?.Invoke();
    }

    [ServerRpc]
    private void OnFailedEventHanlderServerRpc()
    {
        OnFailedEventHanlderClientRpc();
    }

    [ClientRpc]
    private void OnFailedEventHanlderClientRpc()
    {
        OnFailedGameStartEvent?.Invoke();
    }




    public void OnAddUser(UserData data)
    {
        //이름 생성
        NameContainer.CreateUserData(data);

        _userDataList.Add(data);

        //기존에 있던 유저가 이미 선택한 상황이라면
        if (data.turnType != TurnType.None)
        {
            OnValueChangedUser(data);

        } //end if
    }

    public void OnRemoveUser(UserData data)
    {
        NameContainer.RemoveUserData(data);

        if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
        {
            button.OnLeave();
            _userDataList.Remove(data);
        }
    }

    public void OnValueChangedUser(UserData data)
    {
        NameContainer.ChangedUserData(data);

        if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
        {
            button.OnConfirm();

            int idx = FindIndex(data.clientId);
            if (idx < 0) return;

            _userDataList[idx] = data;


            //내가 고를려고 했던 것을 골랐을때 None으로 취소시켜줌
            if (data.turnType == SelectTurnType)
            {
                SelectTurnType = TurnType.None;
                ColorSelectButton.interactable = false;

            } //end if
        } //end if
    }

    public void OnColorConfirm()
    {
        //선택했으면 다 꺼줌
        ColorButtonList.ForEach(btn => btn.SetInteractable(false));

        TurnManager.Instance.SetTurnType(SelectTurnType);

        //서버에 RPC날려서 모든 클라에 적용
        DataController.ColorConfirm(SelectTurnType);

    }

    private void HandleSelectedeColor(LobbyColorButton button)
    {
        SelectTurnType = button.ColorType;
        ColorSelectButton.interactable = true;
        ColorButtonList.ObjExcept(button, btns => btns.OnDeSelected());
    }

    /// <summary>
    /// 모든 플레이어가 준비가 되었는지 확인하는 함수
    /// </summary>
    private bool CheckAllReady()
    {
        //모든 클라이언트가 레디를 해야함
        return _userDataList.TrueForAll(data => data.turnType != TurnType.None);
    }

    private int FindIndex(ulong clientID)
    {
        for (int i = 0; i < _userDataList.Count; ++i)
        {
            if (_userDataList[i].clientId != clientID) continue;

            return i;
        }

        return -1;
    }


    public void OnDestroy()
    {
        //base.OnDestroy();

        foreach (var button in ColorButtonList)
        {
            button.OnClickEvent -= HandleSelectedeColor;

        } //end foreach

        _colorToButtonDic.TryClear();
        _userDataList.TryClear();

        DataController.RemoveResiter(this);
    }
}
