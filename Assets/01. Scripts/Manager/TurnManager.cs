using Unity.Netcode;
using UnityEngine;

/// <summary>
/// -1을 쓰기 위해서 sbyte로 지정
/// </summary>
public enum TurnType : sbyte
{
	None = -1,
	Red,
	Blue,
	Yellow,
	Green,
	End
}

public class TurnManager : NetworkMonoSingleton<TurnManager>
{
	public TurnType StartType = TurnType.None;
	public bool IsMyTurn => _turnTypeNV?.Value == TurnType.None;

	private NetworkVariable<TurnType> _turnTypeNV = new();
	private int _turnIdx = -1;

	public override void OnNetworkSpawn()
	{
		OnResiter();
	}

	private void OnResiter()
	{
		_turnTypeNV.OnValueChanged += OnChangedValue;
	}


	public override void OnDestroy()
	{
		base.OnDestroy();

		_turnTypeNV.OnValueChanged -= OnChangedValue;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
			OnTurnChangedNext();
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			GameStart();
		}
	}

	public void GameStart()
	{
		_turnTypeNV.Value = StartType;
	}

	private void OnChangedValue(TurnType prevValue, TurnType newValue)
	{
		if (prevValue == TurnType.None) return;

		Debug.Log(newValue);

		SignalHub.OnChangedTurnEvent?.Invoke(prevValue, newValue);

		//나의 턴일 경우
		if (IsMyTurn)
		{
			SignalHub.OnMyTurnEvent?.Invoke();

		} //end if
	}

	public TurnType GetTurnTarget()
	{
		return _turnTypeNV.Value;
	}

	public void SetTurnTarget(TurnType turnType)
	{
		_turnTypeNV.Value = turnType;
	}

	public void OnTurnChangedNext()
	{
		//턴이 순서대로 로테이션 되게
		_turnIdx = (_turnIdx == (int)TurnType.End - 1) ? 0 : _turnIdx + 1;

		_turnTypeNV.Value = (TurnType)_turnIdx;
	}


}
