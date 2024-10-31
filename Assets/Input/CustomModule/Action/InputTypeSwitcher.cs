namespace Seongho.InputSystem
{
    public static partial class InputManager
    {
        /// <summary>
        /// Input의 타입을 변경하여 InputAction을 변경해줍니다.
        /// </summary>
        /// <param name="type"> InputType </param>
        public static void ChangedInputType(INPUT_TYPE type)
        {
            switch (type)
            {
                case INPUT_TYPE.Player:

                    Input.UI.Disable();
                    Input.Mobile.Enable();

                    break;

                case INPUT_TYPE.UI:

                    Input.Mobile.Disable();
                    Input.UI.Enable();

                    break;
            }
        }
    }
}