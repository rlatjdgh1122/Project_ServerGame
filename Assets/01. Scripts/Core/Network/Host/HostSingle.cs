using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostSingle : MonoSingleton<HostSingle>
{

    public HostGameManager GameManager { get; private set; }
    public NetworkServer NetServer => GameManager.NetServer;

    public void CreateHost()
    {

        GameManager = new HostGameManager();


    }

    private void OnDestroy()
    {
        
        GameManager.Dispose();

    }

}
