using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug�� ���� �����Ͽ� ������ ���� ��� ����׸� ����ϴ��� Ȯ���ϴ� �뵵
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

