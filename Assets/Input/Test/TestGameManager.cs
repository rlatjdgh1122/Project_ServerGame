using Seongho.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    private void Awake()
    {
        InputManager.ChangedInputType(INPUT_TYPE.Player);
    }
}
