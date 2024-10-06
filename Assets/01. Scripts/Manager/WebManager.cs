using Seongho.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : MonoBehaviour
{
	public class GameResult
	{
		public string UserName;
		public int Score;
	}

	public enum MethodType : byte
	{
		GET, POST, PUT, DELETE
	}
	public enum URLType : byte
	{
		ranking,
	}

	private readonly string _baseUrl = "https://localhost:7146/api";

	private void Start()
	{
		GameResult result = new GameResult()
		{
			UserName = "Choi",
			Score = 100,
		};

		StartCoroutine(Co_SendWebRequest(URLType.ranking, MethodType.POST, result, (uwr) =>
		{
			Debug_S.Log("TODO : UI �����ϱ�");
		}));

		StartCoroutine(Co_SendWebRequest(URLType.ranking, MethodType.GET, null, (uwr) =>
		{
			Debug_S.Log("TODO : UI �����ϱ�2");
		}));
	}

	private IEnumerator Co_SendWebRequest(URLType urlType, MethodType methodType, object obj, Action<UnityWebRequest> callback)
	{
		yield return null;

		string sendUrl = $"{_baseUrl}/{urlType}/";
		Debug_S.Log(sendUrl);

		//�� API�� ������ ��, ����ȭ
		byte[] jsonBytes = null;

		if (obj != null)
		{
			//��ü�� json���� ��ȯ
			string jsonStr = JsonUtility.ToJson(obj);

			//json�� byte������ ��ȯ
			jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
		}

		//���� ������ WebRequest ��� �� Unity ���� ����� ���
		var uwr = new UnityWebRequest(sendUrl, methodType.ToString());
		uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
		uwr.downloadHandler = new DownloadHandlerBuffer();

		//�������� jsonŸ������ ����
		uwr.SetRequestHeader("Content-Type", "application/json");

		//����, �ڷ�ƾ ����
		yield return uwr.SendWebRequest();

		if (uwr.result == UnityWebRequest.Result.ConnectionError
			|| uwr.result == UnityWebRequest.Result.ProtocolError)
		{
			Debug_S.LogError($"ErrorType : {uwr.result}, error : {uwr.error}");

		}//end if

		else
		{
			Debug_S.Log($"Recv : {uwr.downloadHandler.text}");
			callback?.Invoke(uwr);

		}//end else

	}
}
