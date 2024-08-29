using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : ExpansionMonoBehaviour, PlayerAction.IPlayerActions
{
    public void OnGrapInput(InputAction.CallbackContext context)
    {
        //왼쪽클릭
    }   

    public void OnStopInput(InputAction.CallbackContext context)
    {
        //스페이스바
    }
}
