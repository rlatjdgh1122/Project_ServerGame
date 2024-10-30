using DG.Tweening;
using TMPro;
using UnityEngine;

public interface IUIWarningText : IUITarget
{
    public void ShowText(string text, float duration);
}

public class WarningUI : UIElement, IUITarget
{
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private TextMeshProUGUI _warningText = null;

    public override void Awake()
    {
        base.Awake();

        UIManager<IUITarget>.ResisterUI(this, gameObject.name);
    }

    public void ShowText(string text, float duration = 2f)
    {
        _warningText.text = text;
        _warningText.alpha = 0f;

        _warningText.DOFade(1f, _fadeDuration)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(duration, () =>
                {
                    _warningText.DOFade(0f, _fadeDuration);
                });
            });
    }
}