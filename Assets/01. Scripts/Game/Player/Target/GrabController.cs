using Define;
using Seongho.InputSystem;

public class GrabController : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler, INetworkSpawnHandler
{
	private IInputHandler<HASH_INPUT_MOBILE> _inputHandler = null;
	private IGrabHandler _grabHandler = null;

	public void Setup(ComponentList list)
	{
		_inputHandler = list.Find<IInputHandler<HASH_INPUT_MOBILE>>();
		_grabHandler = list.Find<IGrabHandler>();
	}

	public void OnSpawn()
	{
		OnRegister();
	}

	public void OnDespawn()
	{

	}

	private void OnRegister()
	{
		_inputHandler.OnRegisterEvent(HASH_INPUT_MOBILE.Touch, FindTarget);
	}

	private void RemoveRegister()
	{
		_inputHandler.RemoveRegisterEvent(HASH_INPUT_MOBILE.Touch, FindTarget);
	}

	private void FindTarget(INPUT_KEY_STATE key, object[] args)
	{
		if (key == INPUT_KEY_STATE.DOWN)
		{
			_grabHandler.Grab();
		}
		else if (key == INPUT_KEY_STATE.UP)
		{
			_grabHandler.GrabStop();
		}
		else if (key == INPUT_KEY_STATE.PRESSING)
		{
			_grabHandler.Grabbing();
		}
	}

	public void OnPlayerStart()
	{

	}

	public void OnPlayerStop()
	{
		_grabHandler.GrabStop();
	}

	private void OnDestroy()
	{
		RemoveRegister();
	}

}
