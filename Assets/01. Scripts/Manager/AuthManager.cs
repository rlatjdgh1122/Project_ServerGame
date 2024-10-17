using Firebase.Auth;
using Newtonsoft.Json;
using ShardData;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TreeEditor;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AuthManager
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


    private async Task CreateUserServerDataWithServerAsync()
    {
        string serverUrl = $"https://localhost:7012/api/UserData/CreateUserData?uid={UId}"; // ����� ������ ���� API

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(serverUrl, null);

            if (response.IsSuccessStatusCode)
            {
                // JSON ������ string���� �б�
                string json = await response.Content.ReadAsStringAsync();

                // JSON�� ulong���� ��ȯ (����: �������� 'PlayerId'�� ��ȯ�Ѵٰ� ����)
                PlayerId = JsonConvert.DeserializeObject<ulong>(json);

                Debug.Log($"UserData ���� ���� : {PlayerId}");

            }

            else
            {
                Debug.LogError("UserData ���� ����: " + response.StatusCode);

            } //end else

        } //end using
    }

    public async Task<UserServerData> GetUserServerDataWithServerAsync()
    {
        string serverUrl = $"https://localhost:7012/api/UserData/GetUserDataByPlayerId?uid={UId}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(serverUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                UserServerData data = JsonConvert.DeserializeObject<UserServerData>(json);

                Debug.Log("������ �������� ����");
                return data;

            } //end if

            else
            {
                Debug.LogError("Token verification failed");
                return default;

            } //end else

        } //end using
    }

    public async Task UpdateUserServerDataWithServerAsync(UserServerData data)
    {
        string serverUrl = $"https://localhost:7012/api/UserData/UpdateUserData?uid={UId}";

        using (HttpClient client = new HttpClient())
        {

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(serverUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("������ ���� ����");

            } //end if

            else
            {
                Debug.LogError("Token verification failed");

            } //end else

        } //end using
    }
}
