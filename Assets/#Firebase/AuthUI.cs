using ShardData;
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

    public void OnCreateUserData()
    {
        AuthManager.Instance.CreateUserData();
    }

    public async void OnUpdateUserData()
    {
        var prevData = await AuthManager.Instance.GetUserServerDataWithServerAsync();
        UserServerData data = new()
        {
            Coin = prevData.Coin + 1000,
            PlayerId = prevData.PlayerId,
            SkinList = prevData.SkinList,
            UserName = "¼ºÈ£",
        };

        await AuthManager.Instance.UpdateUserServerDataWithServerAsync(data);
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
