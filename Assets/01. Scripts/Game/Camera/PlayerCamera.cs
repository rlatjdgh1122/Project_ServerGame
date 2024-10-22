using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerCamera : ExpansionNetworkBehaviour
{
	[SerializeField] private float _zoomSize = 5f;
	private CinemachineVirtualCamera _camera = null;
	private CinemachineConfiner2D _confiner = null;

	private ulong _Id = 0;
	public ulong ClientId => _Id;

	private void Awake()
	{
		_camera = GetComponent<CinemachineVirtualCamera>();
		_confiner = GetComponent<CinemachineConfiner2D>();
	}

	private void Start()
	{
		_camera.m_Lens.OrthographicSize = _zoomSize;
		_confiner.m_BoundingShape2D = FindObjectOfType<MapConfiner>().GetConfiner();
	}

	public void SetTarget(Player player, ulong clientId)
	{
		_Id = clientId;
		_camera.Follow = player.transform;
	}

	public void SetPriority(int value)
	{
		_camera.Priority = value;
	}


}
