using Define;
using Seongho.InputSystem;

public class GrabController : ExpansionMonoBehaviour, ISetupHandler
{

    private IInputHandler<HASH_INPUT_PLAYER> _inputHandler = null;
    private IGrabHandler _grabHandler = null;
    public void Setup(ComponentList list)
    {
        _inputHandler = list.Find<IInputHandler<HASH_INPUT_PLAYER>>();
        _grabHandler = list.Find<IGrabHandler>();

        _inputHandler.OnRegisterEvent(HASH_INPUT_PLAYER.LeftClick, FindTarget);
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


    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.RemoveRegisterEvent(HASH_INPUT_PLAYER.LeftClick, FindTarget);
        }
    }

}
