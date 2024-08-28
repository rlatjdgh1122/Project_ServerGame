using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class Feedback : MonoBehaviour, IFeedback
{

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    public abstract bool StartFeedback(); //�ǵ�� ����
    public abstract bool FinishFeedback(); //�ǵ�� ����

    protected virtual void OnDestroy()
    {
        FinishFeedback();
    }

    protected virtual void OnDisable()
    {
        FinishFeedback();
    }
}
