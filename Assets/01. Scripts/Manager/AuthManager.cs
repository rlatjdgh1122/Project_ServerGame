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

    public async void Create(string email, string password)
    {
        try
        {
            var authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            Debug.Log("회원가입 완료");

            // 회원가입을 하면 UId를 세팅함
            UId = await GetUIdWithServerAsync();

            // 회원가입을 할 때 UserServerData 생성
            // 추후 : PlayerId를 로그인할 때 가져오는 것이 있어야함
            await UserServerDataManager.Instance.CreateUserServerDataWithServerAsync();
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("회원가입 취소");
        }
    }

    public async void Login(string email, string password)
    {
        try
        {
            var authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            Debug.Log("로그인 완료");

            // UId
            UId = await GetUIdWithServerAsync();

        } //end try

        catch (OperationCanceledException)
        {
            Debug.LogError("로그인 취소");

        } //end catch

        catch (Exception ex)
        {
            Debug.LogError($"로그인 실패: {ex.Message}");

        } //end catch
    }

    public void Logout()
    {
        _auth.SignOut();
    }

    #region UId가져오는 함수

    private async Task<string> GetUIdWithServerAsync()
    {
        FirebaseUser user = _auth.CurrentUser;

        if (user == null)
        {
            Debug.LogError("유저를 찾을 수 업습니다.");

            return null;
        } //end if

        try
        {
            // Request a fresh token
            string idToken = await user.TokenAsync(true);

            // Use the idToken to verify with the server
            return await IdTokenWithServerAsync(idToken);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching token: {ex.Message}");
            return null;
        }
    }
    
    private async Task<string> IdTokenWithServerAsync(string idToken)
    {
        string url = GetURL("verifyToken");

        using (HttpClient client = new HttpClient())
        {
            // Add the token in the Authorization header
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {idToken}");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Get the UID from the response
                    string UId = await response.Content.ReadAsStringAsync();
                    Debug.Log("Token verification succeeded, UId: " + UId);
                    return UId;
                }
                else
                {
                    Debug.LogError("Token verification failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during token verification: {ex.Message}");
                return null;
            }
        }
    }

    #endregion

}
