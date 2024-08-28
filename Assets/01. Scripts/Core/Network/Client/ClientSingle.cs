using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientSingle : MonoSingleton<ClientSingle>
{

    public ClientGameManager GameManager { get; private set; }

    public void CreateClient()
    {

        GameManager = new ClientGameManager(NetworkManager.Singleton);

    }

    private void OnDestroy()
    {
        GameManager.Disconnect();
    }

}
