using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seongho.InputSystem
{
    public class InputMachine<T> : IInputHandler<T> where T : Enum
    {
        private Dictionary<T, InputParams> _inputEventDic = new();                        //Enum타입을 통해 이벤트를 저장
        private Dictionary<T, INPUT_KEY_STATE> _inputStateDic = new();                    //Enum타입을 통해 키의상태 연결 
        private Dictionary<T, Coroutine> _inputCoroutineDic = new();                      //Enum타입을 통해   코루틴 연결 

        /// <summary>
        /// 인덱서로 키상태를 가져옵니다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public INPUT_KEY_STATE this[T key]
        {
            get
            {
                return _inputStateDic[key];
            }
        }

        /// <summary>
        /// Key추가 or Action추가
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void OnRegisterEvent(T key, InputParams action)
        {
            if (!_inputEventDic.ContainsKey(key))
            {

                _inputEventDic.Add(key, action);

                //처음엔 NOT_PRESSED상태로 초기화
                _inputStateDic.Add(key, INPUT_KEY_STATE.NOT_PRESSED);
            }
            else
            {

                _inputEventDic[key] += action;
            }

        }

        /// <summary>
        /// 액션 제거
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void RemoveRegisterEvent(T key, InputParams action)
        {

            if (_inputEventDic[key] == null)
            {
                _inputEventDic.Remove(key);
                _inputStateDic.Remove(key);
                _inputCoroutineDic.Remove(key);
            }
            else
            {
                _inputEventDic[key] -= action;
            }

        }

        /// <summary>
        /// 실제로 인풋을 처리
        /// </summary>
        /// <param name="type">인풋 타입</param>
        /// <param name="context"></param> 
        public void InputRunning(T type, InputAction.CallbackContext context, bool useHolding = false, params object[] @parmas)
        {
            //만약 없다면 리턴해줍니다.
            if (!_inputEventDic.ContainsKey(type)) return;

            //입력을 받았다면 Down상태로 넘어갑니다.
            if (context.performed)
            {
                _inputStateDic[type] = INPUT_KEY_STATE.DOWN;
                _inputEventDic[type]?.Invoke(INPUT_KEY_STATE.DOWN, @parmas);

                if (useHolding) //누르고 있는지를 체크한다면
                {
                    //CallWaitForStopCorouine을 사용하여 키의상태가 Up이 될때까지 PRESSING상태로 넘겨줍니다.
                    //코루틴을 반환하여 저장합니다.
                    Coroutine corou = CoroutineUtil.CallWaitForStopCorouine(
                            () =>
                            {

                                _inputStateDic[type] = INPUT_KEY_STATE.PRESSING;
                                _inputEventDic[type]?.Invoke(INPUT_KEY_STATE.PRESSING, @parmas);

                            },
                            0.02f
                        );

                    //코루틴을 저장합니다.
                    _inputCoroutineDic.Add(type, corou);
                }


            } //end performed

            if (context.canceled)
            {
                //만약 키입력이 취소됐다면 Up상태로 넘어갑니다.
                _inputStateDic[type] = INPUT_KEY_STATE.UP;
                _inputEventDic[type]?.Invoke(INPUT_KEY_STATE.UP, @parmas);

                if (useHolding) //누르고 있는지를 체크한다면
                {
                    //저장한 코루틴을 가져와 즉시 멈춰줍니다.
                    if (_inputCoroutineDic.TryGetValue(type, out var corou))
                    {
                        CoroutineUtil.StopCoroutine(corou);
                        _inputCoroutineDic.Remove(type);
                    }
                }

            } //end canceled

        } // end method
    } // end class
} //end namespace
