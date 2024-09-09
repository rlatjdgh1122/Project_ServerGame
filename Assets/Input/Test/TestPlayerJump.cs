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
            _input = GetComponent<IInputHandler<HASH_INPUT_PLAYER>>();
            _rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// 이벤트를 연결해줍니다.
        /// </summary>
        private void Start()
        {
            _input.OnRegisterEvent(HASH_INPUT_PLAYER.Space, OnJump);
        }

        /// <summary>
        /// 이벤트를 해체해줍니다.
        /// </summary>
        private void OnDestroy()
        {
            _input.RemoveRegisterEvent(HASH_INPUT_PLAYER.Space, OnJump);
        }

        private void OnJump(INPUT_KEY_STATE key, object[] args)
        {
            //눌렸을 경우
            if (key == INPUT_KEY_STATE.DOWN)
            {
                //위로 힘을 가해 점프 구현
                _rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            }
        }

    }
}