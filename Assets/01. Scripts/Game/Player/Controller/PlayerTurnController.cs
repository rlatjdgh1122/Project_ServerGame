using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnController : ExpansionMonoBehaviour, ISetupHandler, ITurnChangedHandler
{
    private IPlayerStopHandler playerStop = null;

    public void Setup(ComponentList list)
    {
        playerStop = list.Find<IPlayerStopHandler>();
    }


}
