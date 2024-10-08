using System;
using TMPro;
using UnityEngine.UI;

public class LobbyColorButton : ExpansionMonoBehaviour
{
	public TurnType ColorType = TurnType.None;

	private Button _btn = null;
	private TextMeshProUGUI _text = null;
	private bool _isSelected = false;

	public event Action<LobbyColorButton> OnClickEvent = null;

	private void Awake()
	{
		_btn = GetComponent<Button>();
		_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
	}

	public void OnClick()
	{
		_text.text = "º±≈√µ ";
		OnClickEvent?.Invoke(this);
	}

	public void OnConfirm()
	{
		OnSelected();

		_isSelected = true;
	}

	public void OnLeave()
	{
		_isSelected = false;

		OnDeSelected();
	}

	public void OnSelected()
	{
		if (_isSelected) return;

		_text.text = "º±≈√µ ";
		_btn.interactable = false;
	}

	public void OnDeSelected()
	{
		if (_isSelected) return;

		_text.text = $"{ColorType}";
		_btn.interactable = true;
	}

	public void SetInteractable(bool value)
	{
		_btn.interactable = value;
	}

}
