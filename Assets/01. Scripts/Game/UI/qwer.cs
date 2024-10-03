using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class qwer : MonoBehaviour
{
    public TextMeshProUGUI _joinCodeText = null;

    private void Start()
    {
        _joinCodeText.text = string.Format("CODE: {0}", HostSingle.Instance.GameManager.JoinCode);
    }
}
