using System.Collections.Generic;
using UnityEngine;

public class FeedbackPlayer : MonoBehaviour
{
    public bool IsSuccessed { get; set; } = true;

    private List<Feedback> _feedbackToPlay = null;
    public List<Feedback> FeedbackToPlay => _feedbackToPlay;

    private void Awake()
    {
        SetUpEffect();
    }

    public void SetUpEffect()
    {
        _feedbackToPlay = new List<Feedback>();
        GetComponents<Feedback>(_feedbackToPlay);
    }

    public void PlayFeedback()
    {
        FinishFeedback();
        foreach (Feedback f in _feedbackToPlay)
        {

            //�ϳ��� False�� false�� ����
            IsSuccessed = f.StartFeedback();
        }
    }

    public void FinishFeedback()
    {
        foreach (Feedback f in _feedbackToPlay)
        {
            f.FinishFeedback();
        }
    }
}
