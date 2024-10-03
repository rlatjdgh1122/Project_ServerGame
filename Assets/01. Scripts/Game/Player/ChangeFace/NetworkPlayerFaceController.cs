using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Netcode for Unity 사용

public class NetworkPlayerFaceController : ExpansionNetworkBehaviour, ISetupHandler, INetworkSpawnHandler
{
    private ILocalActionHandler<FaceType> _localActionHanlder = null;

    public void Setup(ComponentList list)
    {
        _localActionHanlder = list.Find<ILocalActionHandler<FaceType>>();
    }

    public void OnSpawn()
    {
        if (!IsOwner) return;

        _localActionHanlder.OnActionEvent += OnActionEvent;
    }

    public void OnDespawn()
    {
        if (!IsOwner) return;

        _localActionHanlder.OnActionEvent -= OnActionEvent;
    }

    private void OnActionEvent(FaceType type)
    {
        // 로컬에서 액션이 발생할 때 RPC 호출
        SubmitActionRequestServerRpc(type);
    }

    // 서버로 액션 전달
    [ServerRpc]
    private void SubmitActionRequestServerRpc(FaceType type)
    {
        // 서버에서 모든 클라이언트에게 RPC 호출
        BroadcastActionClientRpc(type);
    }

    // 모든 클라이언트에서 액션을 실행
    [ClientRpc]
    private void BroadcastActionClientRpc(FaceType type)
    {
        _localActionHanlder.DoAction(type);
    }
}
