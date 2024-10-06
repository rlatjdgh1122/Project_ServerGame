using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class LobbyManager : ExpansionNetworkBehaviour
{
    [SerializeField] private UnityEvent OnSucceededGameStartEvent = null;
    [SerializeField] private UnityEvent OnFailedGameStartEvent = null;

    public void OnGameStart()
    {
        bool isCheck = false;

        //�÷��̾���� Ÿ���� �����ߴ���
        foreach (var id in NetworkManager.ConnectedClientsIds)
        {
            UserData? data = HostSingle.Instance.NetServer.GetUserDataByClientID(id);

            //�Ѹ��̶� �������� �ʾҴٸ�
            if (data.Value.turnType == TurnType.None)
            {
                isCheck = true;
                break;
            }

        } //end foreach

        if (isCheck)   OnFailedGameStartEvent?.Invoke();
        else        OnSucceededGameStartEvent?.Invoke();
    }
}
