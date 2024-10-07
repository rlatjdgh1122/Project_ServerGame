using System;
using TMPro;
using UnityEngine.UI;

public class LobbyColorButton : ExpansionMonoBehaviour
{
	public TurnType ColorType = TurnType.None;

	private bool _isSelect = false;
	public Button Button = null;
	private TextMeshProUGUI Text = null;
	public event Action<TurnType> OnClickEvent = null;

	private void Awake()
	{
		Button = GetComponent<Button>();
		Text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
	}

	public void OnClick()
	{
		_isSelect = !_isSelect;
		Text.text = _isSelect == true ? "º±≈√µ " : $"{ColorType}";
		OnClickEvent?.Invoke(ColorType);
	}

}
