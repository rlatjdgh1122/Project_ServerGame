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
			Debug_S.Log("TODO : UI 갱신하기");
		}));

		StartCoroutine(Co_SendWebRequest(URLType.ranking, MethodType.GET, null, (uwr) =>
		{
			Debug_S.Log("TODO : UI 갱신하기2");
		}));
	}

	private IEnumerator Co_SendWebRequest(URLType urlType, MethodType methodType, object obj, Action<UnityWebRequest> callback)
	{
		yield return null;

		string sendUrl = $"{_baseUrl}/{urlType}/";
		Debug_S.Log(sendUrl);

		//웹 API로 보내기 전, 직렬화
		byte[] jsonBytes = null;

		if (obj != null)
		{
			//객체를 json으로 변환
			string jsonStr = JsonUtility.ToJson(obj);

			//json을 byte형으로 변환
			jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
		}

		//여러 버전의 WebRequest 방법 중 Unity 지원 방식을 사용
		var uwr = new UnityWebRequest(sendUrl, methodType.ToString());
		uwr.uploadHandler = new UploadHandlerRaw(jsonBytes);
		uwr.downloadHandler = new DownloadHandlerBuffer();

		//데이터의 json타입으로 설정
		uwr.SetRequestHeader("Content-Type", "application/json");

		//전송, 코루틴 실행
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
