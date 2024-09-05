using System;

public static class InputManager
{
    public enum InputType
    {
        Player = 0,
        UI,
    }

    public static PlayerAction Input = null;

    static InputManager()
    {
        Input = new();
    }

    public static void ChangedInputType(InputType type)
    {
        switch (type)
        {
            case InputType.Player:

                Input.UI.Disable();
                Input.Player.Enable();

                break;

            case InputType.UI:

                Input.Player.Disable();
                Input.UI.Enable();

                break;
        }
    }

    public static void CreateMachine<T>(out InputMachine<T> inputMachine) where T : Enum
    {
        inputMachine = new();
    }
}
