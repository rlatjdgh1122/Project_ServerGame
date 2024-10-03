using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; // Netcode for Unity ���

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
        // ���ÿ��� �׼��� �߻��� �� RPC ȣ��
        SubmitActionRequestServerRpc(type);
    }

    // ������ �׼� ����
    [ServerRpc]
    private void SubmitActionRequestServerRpc(FaceType type)
    {
        // �������� ��� Ŭ���̾�Ʈ���� RPC ȣ��
        BroadcastActionClientRpc(type);
    }

    // ��� Ŭ���̾�Ʈ���� �׼��� ����
    [ClientRpc]
    private void BroadcastActionClientRpc(FaceType type)
    {
        _localActionHanlder.DoAction(type);
    }
}
