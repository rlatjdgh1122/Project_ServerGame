using Seongho.InputSystem;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private INPUT_TYPE _initInputType = INPUT_TYPE.Player;

    public void Awake()
    {
        InputManager.ChangedInputType(_initInputType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            InputManager.ChangedInputType(INPUT_TYPE.UI);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            InputManager.ChangedInputType(INPUT_TYPE.Player);
        }
    }
}
