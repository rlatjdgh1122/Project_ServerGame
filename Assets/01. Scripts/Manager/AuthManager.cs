using Firebase.Auth;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AuthManager
{
    private string _uid = "";
    public string UID
    {
        get
        {
            if (string.IsNullOrEmpty(_uid))
            {
                Debug.Log("UID�� ã�� �� �����ϴ�.");

                return "";

            } //end if

            return _uid;

        } //end get
    }

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
                Debug.Log("�α׾ƿ�");

            } //end if

            _user = _auth.CurrentUser;

            if (signed)
            {
                Debug.Log("�α���");

            } //end if
        } //end if

    }

    public void Create(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("ȸ������ ���");
                return;

            } //end if

            if (task.IsFaulted)
            {
                Debug.LogError("ȸ������ ����");
                return;

            } //end if

            FirebaseUser newUser = task.Result.User;
            Debug.Log("ȸ������ �Ϸ�");
        });
    }

    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�α��� ���");
                return;

            } //end if

            if (task.IsFaulted)
            {
                Debug.LogError("�α��� ����");
                return;

            } //end if

            FirebaseUser newUser = task.Result.User;
            Debug.Log("�α��� �Ϸ�");

            _ = GetUIdByServerAsync();
        });
    }

    public void Logout()
    {
        _auth.SignOut();
    }

    private async Task GetUIdByServerAsync()
    {
        FirebaseUser user = _auth.CurrentUser;

        await user.TokenAsync(true).ContinueWith(async task =>
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

            await IdTokenWithServerAsync(idToken);
        });
    }

    private async Task IdTokenWithServerAsync(string idToken)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {idToken}");

            string serverUrl = "https://localhost:7012/api/Auth/verifyToken";
            HttpResponseMessage response = await client.GetAsync(serverUrl);

            _uid = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Token verification succeeded");
            } //end if

            else
            {
                Debug.LogError("Token verification failed");

            } //end else
        } //end method
    } //end class
}
