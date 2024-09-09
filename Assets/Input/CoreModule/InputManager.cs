using System;

namespace Seongho.InputSystem
{
    public static partial class InputManager
    {

        public static PlayerAction Input = null;

        static InputManager()
        {
            Input = new();
        }

        /// <summary>
        /// 직접만든 InputMachine를 생성해줍니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputMachine"></param>
        public static void CreateMachine<T>(out InputMachine<T> inputMachine) where T : Enum
        {
            inputMachine = new();
        }
    }

}

