using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AuthManager : RestSingleton<AuthManager>
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

            _ = GetUIdWithServerAsync();
        });
    }

    public void Logout()
    {
        _auth.SignOut();
    }

    #region UId가져오는 함수

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

    #endregion

    /// <summary>
    /// 계정이 생성될 때 한번 실행
    /// </summary>
    private async Task CreateUserServerDataWithServerAsync()
    {
        string url = GetURL("CreateUserData", new FromData() { Name = "uid", Data = UId });

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                // JSON 응답을 string으로 읽기
                string json = await response.Content.ReadAsStringAsync();

                //고유 넘버인 PlayerId를 서버에서 받아옴
                PlayerId = JsonConvert.DeserializeObject<ulong>(json);

                Debug.Log($"UserData 생성 성공 : {PlayerId}");

            }

            else
            {
                Debug.LogError("UserData 생성 실패: " + response.StatusCode);

            } //end else

        } //end using
    }

}
