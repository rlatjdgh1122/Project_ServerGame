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

        //플레이어들이 타입을 선택했는지
        foreach (var id in NetworkManager.ConnectedClientsIds)
        {
            UserData? data = HostSingle.Instance.NetServer.GetUserDataByClientID(id);

            //한명이라도 선택하지 않았다면
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
