using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug를 직접 정의하여 참조를 통해 어디서 디버그를 사용하는지 확인하는 용도
/// </summary>
public static class Debug_S
{
	public static void Log(object msg)
	{
#if UNITY_EDITOR
		Debug.Log(msg);
#endif
	}

	public static void LogError(object msg)
	{
#if UNITY_EDITOR
		Debug.LogError(msg);
#endif
	}
}

