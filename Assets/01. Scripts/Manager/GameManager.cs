using Seongho.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private INPUT_TYPE _initInputType = INPUT_TYPE.Player;

	public void Awake()
	{
		InputManager.ChangedInputType(_initInputType);
	}
}
