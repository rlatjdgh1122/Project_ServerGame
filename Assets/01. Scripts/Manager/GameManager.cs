using Seongho.InputSystem;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private INPUT_TYPE _initInputType = INPUT_TYPE.Player;

    public void Start()
    {
        InputManager.ChangedInputType(_initInputType);
    }

}
