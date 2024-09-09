using UnityEngine;

namespace Seongho.InputSystem
{
    public class TestPlayerJump : MonoBehaviour
    {
        [Range(5f, 15f)]
        public float JumpPower = 10f;

        private IInputHandler<HASH_INPUT_PLAYER> _input = null;
        private Rigidbody2D _rb = null;

        private void Awake()
        {
            _input = GetComponent<TestPlayerInput>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _input.OnRegisterEvent(HASH_INPUT_PLAYER.Space, OnJump);
            _input.OnRegisterEvent(HASH_INPUT_PLAYER.LeftClick, OnClick);
        }

        private void OnDestroy()
        {
            _input.RemoveRegisterEvent(HASH_INPUT_PLAYER.Space, OnJump);
            _input.RemoveRegisterEvent(HASH_INPUT_PLAYER.LeftClick, OnClick);
        }

        private void OnJump(INPUT_KEY_STATE key, object[] args)
        {
            if (key == INPUT_KEY_STATE.DOWN)
            {
                _rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            }
        }

        private void OnClick(INPUT_KEY_STATE key, object[] args)
        {
            if (key == INPUT_KEY_STATE.DOWN)
            {
                Debug.Log("클릭됨");
            }
            else if (key == INPUT_KEY_STATE.PRESSING)
            {
                Debug.Log("클릭 중");
            }
            else if (key == INPUT_KEY_STATE.UP)
            {
                Debug.Log("클릭 중지");
            }
        }


    }
}