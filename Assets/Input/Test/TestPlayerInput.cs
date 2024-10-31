using UnityEngine;
using UnityEngine.InputSystem;

namespace Seongho.InputSystem
{
    public class TestPlayerInput : MonoBehaviour
    {
/*
        private InputMachine<HASH_INPUT_MOBILE> _inputContainer = null;

        private void Awake()
        {
            //InputMachine을 생성해줍니다.
            InputManager.CreateMachine(out _inputContainer);

            InputSetting();
        }

        /// <summary>
        /// InputAction을 설정해줍니다.
        /// </summary>
        public void InputSetting()
        {
            InputManager.Input.Player.SetCallbacks(this);
        }

        /// <summary>
        /// 이벤트들을 HashKey에 맞춰 연결해줍니다.
        /// </summary>
        public void OnRegisterEvent(HASH_INPUT_MOBILE key, InputParams action)
        {
            _inputContainer.OnRegisterEvent(key, action);
        }

        /// <summary>
        /// 이벤트들을 HashKey에 맞춰 해제해줍니다.
        /// </summary>
        public void RemoveRegisterEvent(HASH_INPUT_MOBILE key, InputParams action)
        {
            _inputContainer.RemoveRegisterEvent(key, action);
        }

        /// <summary>
        /// 키를 매핑해줍니다.
        /// </summary>
        public void OnLeftClickInput(InputAction.CallbackContext context)
        {
            //왼쪽 클릭을 누를 경우 연결된 이벤트 실행

            //파라미터 설명
            // 1. 실행할 HashKey
            // 2. context
            // 3. InputHolding을 해줄것인지 안해줄것인지에 대한 여부
            // 4. 따로 넘겨줄 params object - 여러개의 object를 넘겨줄 수 있음 (비워놔도됨)
            // *꿀팁 : InputHolding을 사용하지 않을 경우엔 false로 해주는 것이 성능상으로 좋다.
            _inputContainer.InputRunning(HASH_INPUT_MOBILE.Touch, context, true);

        }

        public void OnSpaceClickInput(InputAction.CallbackContext context)
        {
            //스페이스 바를 누를 경우 연결된 이벤트 실행

            _inputContainer.InputRunning(HASH_INPUT_MOBILE.Double_Touch, context, false);
        }*/
    }
}