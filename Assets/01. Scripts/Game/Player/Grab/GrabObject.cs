using System;

public class GrabObject : ExpansionMonoBehaviour, IGrabHandler, ISetupHandler
{
    private ISpringJoint2DHandler _joint = null;
    private ILineRenderer2DHandler _line = null;
    private ITargetSensor _targetSensor = null;

    private IGrabTargetHandler _target = null;

    public event Action OnStartGrabEvent = null;
    public event Action OnStartGrabbingEvent = null;
    public event Action OnStopGrabEvent = null;

    public void Setup(ComponentList list)
    {
        _joint = list.Find<ISpringJoint2DHandler>();
        _line = list.Find<ILineRenderer2DHandler>();
        _targetSensor = list.Find<ITargetSensor>();
    }

    public void Grab()
    {
        OnStartGrabEvent?.Invoke();
        DoGrab();
    }

    public void Grabbing()
    {
        OnStartGrabbingEvent?.Invoke();
        DoGrabbing();
    }

    public void GrabStop()
    {
        OnStopGrabEvent?.Invoke();
        DoGrabStop();
    }

    public void DoGrab()
    {
        _target = _targetSensor.FindTarget();

        if (_target != null)
        {
            //��������
            _joint?.SetTarget(_target.GetRigidBody());
            _joint?.SetDistance(_targetSensor.GetDistance());
        }
        else
        {
            //Ÿ���� ������ �ȉ������
        }


    }

    public void DoGrabbing()
    {
        if (_target != null)
        {
            _line.SetTargetPosition(_target.GetPosition());
        }
    }

    public void DoGrabStop()
    {
        //Ÿ���� ��������
        _joint?.SetTarget(null);
        _joint.SetDisable();

        //���η������� ����
        _line?.Clear();
    }

}
