using Firebase.Auth;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AuthManager
{
    private static AuthManager instance = null;
    public static AuthManager Instance
    {
        get
        {
            if (instance == null) instance = new AuthManager();
            return instance;

        } //end get
    }

    private static FirebaseAuth _auth = null;
    private static FirebaseUser _user = null;

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        _auth = FirebaseAuth.DefaultInstance;

        if (_auth != null)
        {
            if (_auth.CurrentUser != null)
                _auth.SignOut();

            _auth.StateChanged += OnChanged;
        } //end if
    }

    private static void OnChanged(object sender, EventArgs e)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signed = _auth.CurrentUser != null;
            if (!signed && _user != null)
            {
                Debug.Log("로그아웃");

            } //end if

            _user = _auth.CurrentUser;

            if (signed)
            {
                Debug.Log("로그인");

            } //end if
        }

    }

    public void Create(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;

            } //end if

            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패");
                return;

            } //end if

            FirebaseUser newUser = task.Result.User;
            Debug.Log("회원가입 완료");
        });
    }

    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return;

            } //end if

            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                return;

            } //end if

            FirebaseUser newUser = task.Result.User;
            Debug.Log("로그인 완료");

            A();
        });
    }

    public void Logout()
    {
        _auth.SignOut();
    }

    private async Task A()
    {
        FirebaseUser user = _auth.CurrentUser;
        _ = user.TokenAsync(true).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }

            string idToken = task.Result;

            // Send token to your backend via HTTPS
            // ...

            VerifyTokenWithServer(idToken);

        });
    }

    private async void VerifyTokenWithServer(string accessToken)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            string serverUrl = "https://localhost:7012/api/Auth/verifyToken";
            HttpResponseMessage response = await client.GetAsync(serverUrl);

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Token verification succeeded");
            }
            else
            {
                Debug.LogError("Token verification failed");
            }
        }
    }
}
