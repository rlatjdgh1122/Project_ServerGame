using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private InputManager.InputType _initInputType = InputManager.InputType.Player;

    public void Awake()
    {
        InputManager.ChangedInputType(_initInputType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            InputManager.ChangedInputType(InputManager.InputType.UI);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            InputManager.ChangedInputType(InputManager.InputType.Player);
        }
    }
}
