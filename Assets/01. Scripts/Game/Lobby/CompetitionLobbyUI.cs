using ExtensionMethod.Dictionary;
using ExtensionMethod.List;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ������ �κ� UI
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
            Debug.Log("Ŭ��� ������ �� ����");
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
        //�̸� ����
        NameContainer.CreateUserData(data);

        _userDataList.Add(data);

        //������ �ִ� ������ �̹� ������ ��Ȳ�̶��
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


            //���� ������ �ߴ� ���� ������� None���� ��ҽ�����
            if (data.turnType == SelectTurnType)
            {
                SelectTurnType = TurnType.None;
                ColorSelectButton.interactable = false;

            } //end if
        } //end if
    }

    public void OnColorConfirm()
    {
        //���������� �� ����
        ColorButtonList.ForEach(btn => btn.SetInteractable(false));

        TurnManager.Instance.SetTurnType(SelectTurnType);

        //������ RPC������ ��� Ŭ�� ����
        DataController.ColorConfirm(SelectTurnType);

    }

    private void HandleSelectedeColor(LobbyColorButton button)
    {
        SelectTurnType = button.ColorType;
        ColorSelectButton.interactable = true;
        ColorButtonList.ObjExcept(button, btns => btns.OnDeSelected());
    }

    /// <summary>
    /// ��� �÷��̾ �غ� �Ǿ����� Ȯ���ϴ� �Լ�
    /// </summary>
    private bool CheckAllReady()
    {
        //��� Ŭ���̾�Ʈ�� ���� �ؾ���
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
