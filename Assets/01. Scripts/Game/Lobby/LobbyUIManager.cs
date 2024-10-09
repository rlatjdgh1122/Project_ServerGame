using ExtensionMethod.Dictionary;
using ExtensionMethod.List;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour, ILobbyManager
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

	private void Start()
	{
		if(!UserDataManager.Instance.IsHost)
		{
			GameStartButton.gameObject.SetActive(false);

		} //end if
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

		//������ �ִ� ������ �̹� ������ ��Ȳ�̶��
		if (data.turnType != TurnType.None)
		{
			OnChangedDataEvent(data);

		} //end if

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

	/// <summary>
	/// �÷��̾ ������ �� ��ư�� �ٽ� ����
	/// </summary>
	private void OnRemoveDataEvent(UserData data)
	{
		if (_colorToButtonDic.TryGetValue(data.turnType, out var button))
		{
			button.OnLeave();
			_userDataList.Remove(data);
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
}
