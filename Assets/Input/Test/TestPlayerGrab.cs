using UnityEngine;

namespace Seongho.InputSystem
{
    public class TestPlayerGrab : MonoBehaviour
    {
        public Transform TargetTrm = null;

        private IInputHandler<HASH_INPUT_PLAYER> _input = null;
        private LineRenderer _lr = null;

        private void Awake()
        {
            _input = GetComponent<IInputHandler<HASH_INPUT_PLAYER>>();
            _lr = GetComponent<LineRenderer>();

            _lr.enabled = false;
        }

        /// <summary>
        /// 이벤트를 연결해줍니다.
        /// </summary>
        private void Start()
        {
            _input.OnRegisterEvent(HASH_INPUT_PLAYER.LeftClick, OnClick);
        }

        /// <summary>
        /// 이벤트를 해체해줍니다.
        /// </summary>
        private void OnDestroy()
        {
            _input.RemoveRegisterEvent(HASH_INPUT_PLAYER.LeftClick, OnClick);
        }

        private void OnClick(INPUT_KEY_STATE key, object[] args)
        {
            if (key == INPUT_KEY_STATE.DOWN)
            {
                _lr.enabled = true;
            }
            else if (key == INPUT_KEY_STATE.PRESSING)
            {
                _lr.SetPosition(0, transform.position);
                _lr.SetPosition(1, TargetTrm.position);
            }
            else if (key == INPUT_KEY_STATE.UP)
            {
                _lr.enabled = false;
            }
        }

    }
}
