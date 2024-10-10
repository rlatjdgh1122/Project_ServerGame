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
		//clientId�� ���� Ű�� ���� ī�޶�� �켱������ �����ְ� ������ ������
		_clientIdToCameraDic.KeyExcept(clientId, result => result.SetPriority(10), other => other.SetPriority(1));
	}
}
