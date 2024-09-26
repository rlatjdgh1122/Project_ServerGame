
using System;
using System.Collections;
using UnityEngine;

public static partial class CoroutineUtil
{
    /// <summary>
    /// [루프 함수] 조건이 만족할때까지 루프합니다.
    /// </summary>
    /// <param name="predicate"> 종료 조건 </param>
    /// <param name="action"> 루프되면서 실행할 Action </param>
    /// <param name="heartBeat"> 반복 주기 </param>
    /// <param name="afterAction"> 종료 후 실행할 Action</param>
    public static void CallWaitForActionUntilTrue(Func<bool> predicate, Action action, float heartBeat = 0.02f, Action afterAction = null)
    {
        EnsureCoroutineExecutor();

        _coroutineExecutor.StartCoroutine(DoCallWaitForActionUntilTrue(heartBeat, predicate, action, afterAction));
    }

	/// <summary>
	/// [루프 함수] 직접 Stop할때까지 루프합니다.
	/// CorouineUtil.StopCoroutine을 사용하여 외부에서 종료해줍니다.
	/// </summary>
	/// <param name="action"> 루프되면서 실행할 Action </param>
	/// <param name="heartBeat"> 반복 주기 </param>
	/// <returns> 코루틴 </returns>
	public static Coroutine CallWaitForStopCorouine(Action action, float heartBeat = 0.02f)
    {
        EnsureCoroutineExecutor();

        return _coroutineExecutor.StartCoroutine(DoCallWaitForStopCorouine(heartBeat, action));
    }

    /// <summary>
    /// 코루틴을 멈춥니다.
    /// CallWaitForStopCorouine와 호환됩니다.
    /// </summary>
    /// <param name="coroutine"> 실행종료 할 코루틴 </param>
    /// <param name="afterAction"> 종료 후 실행할 Action </param>
    public static void StopCoroutine(Coroutine coroutine, Action afterAction = null)
    {
        _coroutineExecutor.StopCoroutine(coroutine);
        afterAction?.Invoke();
    }

    private static IEnumerator DoCallWaitForActionUntilTrue(float heartBeat, Func<bool> predicate, Action action, Action afterAction)
    {
        while (!predicate())
        {
            yield return new WaitForSeconds(heartBeat);
            action?.Invoke();
        }

        afterAction?.Invoke();
    }

    private static IEnumerator DoCallWaitForStopCorouine(float heartBeat, Action action)
    {
        while (true)
        {
            yield return new WaitForSeconds(heartBeat);
            action?.Invoke();
        }
    }
}


