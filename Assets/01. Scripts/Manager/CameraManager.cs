using ExtensionMethod.Dictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : ExpansionMonoBehaviour, IGameFlowHandler
{
    [SerializeField] private PlayerCamera _camera = null;

    void IGameFlowHandler.OnGameStart()
    {
        _camera = Instantiate(_camera);
    }

    public void ShowPlayerCamera(ulong clientId)
    {

    }
}
