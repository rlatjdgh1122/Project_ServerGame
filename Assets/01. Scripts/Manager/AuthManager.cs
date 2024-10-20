using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AuthManager : RestSingleton<AuthManager>
{
    private string _uid = "";
    private ulong _playerid = long.MaxValue; //������ long.MaxValue �̸��� ��

    #region Property

    public string UId
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

        private set
        {
            _uid = value;

        } //end set
    }

    public ulong PlayerId
    {
        get
        {
            if (_playerid.Equals(long.MaxValue))
            {
                Debug.Log("Playerid�� ã�� �� �����ϴ�.");

                return default;

            } //end if

            return _playerid;

        } //end get

        private set
        {
            _playerid = value;

        } //end set
    }
    #endregion

    public AuthManager() : base("Auth") { }

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

    public void CreateUserData()
    {
        _ = CreateUserServerDataWithServerAsync();
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

            _ = GetUIdWithServerAsync();
        });
    }

    public void Logout()
    {
        _auth.SignOut();
    }

    #region UId�������� �Լ�

    private async Task GetUIdWithServerAsync()
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
        string url = GetURL("verifyToken");
        Debug.Log(url);
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {idToken}");

            HttpResponseMessage response = await client.GetAsync(url);

            UId = await response.Content.ReadAsStringAsync(); //�������� ������ UID�� �޾ƿ�
                
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

    #endregion

    /// <summary>
    /// ������ ������ �� �ѹ� ����
    /// </summary>
    private async Task CreateUserServerDataWithServerAsync()
    {
        string url = GetURL("CreateUserData", new FromData() { Name = "uid", Data = UId });

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                // JSON ������ string���� �б�
                string json = await response.Content.ReadAsStringAsync();

                //���� �ѹ��� PlayerId�� �������� �޾ƿ�
                PlayerId = JsonConvert.DeserializeObject<ulong>(json);

                Debug.Log($"UserData ���� ���� : {PlayerId}");

            }

            else
            {
                Debug.LogError("UserData ���� ����: " + response.StatusCode);

            } //end else

        } //end using
    }

}
