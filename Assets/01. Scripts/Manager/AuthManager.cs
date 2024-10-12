using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public TextMeshProUGUI _logText = null;
    public TextMeshProUGUI token = null;

    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void OnSignIn()
    {
        await SignInAnonymous();
    }

    public void OnSignOut()
    {
        _logText.text = "pleaze Click to LoginButton";
        token.text = $"토큰 : {GetAccessToken()}";
        AuthenticationService.Instance.SignOut(true);
    }

    private async Task SignInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            token.text = $"토큰 : {GetAccessToken()}";
            _logText.text = $"Player Id : {AuthenticationService.Instance.PlayerId}";
        }
        catch (AuthenticationException ex)
        {
            _logText.text = "Sign in Failed!";
            Debug.LogException(ex);
        }
    }

    public string GetAccessToken()
    {
        return AuthenticationService.Instance.AccessToken;
    }
}
