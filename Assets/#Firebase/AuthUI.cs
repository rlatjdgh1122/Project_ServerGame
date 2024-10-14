using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthUI : MonoBehaviour
{
    public TMP_InputField EmailInput = null;
    public TMP_InputField PasswordInput = null;

    public void OnCreate()
    {
        AuthManager.Instance.Create(EmailInput.text, PasswordInput.text);
    }

    public void OnLogin()
    {
        AuthManager.Instance.Login(EmailInput.text, PasswordInput.text);
    }

    public void OnLogout()
    {
        AuthManager.Instance.Logout();
    }
}
