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
    private ulong _playerid = long.MaxValue; //무조건 long.MaxValue 미만의 값

    #region Property

    public string UId
    {
        get
        {
            if (string.IsNullOrEmpty(_uid))
            {
                Debug.Log("UID를 찾을 수 없습니다.");

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
                Debug.Log("Playerid를 찾을 수 없습니다.");

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
                Debug.Log("로그아웃");

            } //end if

            _user = _auth.CurrentUser;

            if (signed)
            {
                Debug.Log("로그인");

            } //end if
        } //end if

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

            UId = await response.Content.ReadAsStringAsync(); //서버에서 전달한 UID를 받아옴

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
        string serverUrl = $"https://localhost:7012/api/UserData/CreateUserData?uid={UId}"; // 사용자 데이터 생성 API

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(serverUrl, null);

            if (response.IsSuccessStatusCode)
            {
                // JSON 응답을 string으로 읽기
                string json = await response.Content.ReadAsStringAsync();

                // JSON을 ulong으로 변환 (가정: 서버에서 'PlayerId'를 반환한다고 가정)
                PlayerId = JsonConvert.DeserializeObject<ulong>(json);

                Debug.Log($"UserData 생성 성공 : {PlayerId}");

            }

            else
            {
                Debug.LogError("UserData 생성 실패: " + response.StatusCode);

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

                Debug.Log("데이터 가져오기 성공");
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
                Debug.Log("데이터 수정 성공");

            } //end if

            else
            {
                Debug.LogError("Token verification failed");

            } //end else

        } //end using
    }
}
