using ExtensionMethod.Dictionary;
using System.Collections.Generic;

public class CameraManager : NetworkMonoSingleton<CameraManager>
{
	private Dictionary<ulong, PlayerCamera> _clientIdToCameraDic = new();

	public void Add(ulong id, PlayerCamera camera)
	{
		_clientIdToCameraDic.Add(id, camera);
	}

	public void ShowPlayerCamera(ulong clientId)
	{
		//clientId와 같은 키를 가진 카메라는 우선순위를 높혀주고 나머진 낮혀줌
		_clientIdToCameraDic.KeyExcept(clientId, result => result.SetPriority(10), other => other.SetPriority(1));
	}
}
