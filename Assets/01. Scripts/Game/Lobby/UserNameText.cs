using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserNameText : MonoBehaviour   
{
    private TextMeshProUGUI _text = null;

	private void Awake()
	{
		_text =GetComponent<TextMeshProUGUI>();
	}

	public void SetName(string name)
    {
        _text.text = name;
    }

    public void SetColor(TurnType turnType)
    {
        _text.color = ColorController.GetColorByTurnType(turnType);

	}
}
