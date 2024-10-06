using TMPro;
using UnityEngine.UI;

public class LobbyColorButton : ExpansionMonoBehaviour
{
    public TurnType ColorType = TurnType.None;

    private bool _isSelect = false;
    private Button _button = null;
    private TextMeshProUGUI _text = null;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void OnClick()
    {
        _isSelect = !_isSelect;
        _text.text = _isSelect == true ? "º±≈√µ " : $"{ColorType}";
        PlayerManager.Instance.SetTurnType(ColorType);
    }

}
