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

    public abstract bool StartFeedback(); //피드백 생성
    public abstract bool FinishFeedback(); //피드백 종료

    protected virtual void OnDestroy()
    {
        FinishFeedback();
    }

    protected virtual void OnDisable()
    {
        FinishFeedback();
    }
}
