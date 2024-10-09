using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinCodeUI : MonoBehaviour
{
    public TextMeshProUGUI JoinCodeText = null;

    private void Start()
    {
        JoinCodeText.text = string.Format("CODE: {0}", HostSingle.Instance.GameManager.JoinCode);
    }

    public void OnCopyToClipBoard()
    {
        GUIUtility.systemCopyBuffer = HostSingle.Instance.GameManager.JoinCode;
    }
}
