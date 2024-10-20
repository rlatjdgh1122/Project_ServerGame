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

    public async void Create(string email, string password)
    {
        try
        {
            var authResult = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            Debug.Log("ȸ������ �Ϸ�");

            // ȸ�������� �ϸ� UId�� ������
            UId = await GetUIdWithServerAsync();

            // ȸ�������� �� �� UserServerData ����
            // ���� : PlayerId�� �α����� �� �������� ���� �־����
            await UserServerDataManager.Instance.CreateUserServerDataWithServerAsync();
        }
        catch (OperationCanceledException)
        {
            Debug.LogError("ȸ������ ���");
        }
    }

    public async void Login(string email, string password)
    {
        try
        {
            var authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            Debug.Log("�α��� �Ϸ�");

            // UId
            UId = await GetUIdWithServerAsync();

        } //end try

        catch (OperationCanceledException)
        {
            Debug.LogError("�α��� ���");

        } //end catch

        catch (Exception ex)
        {
            Debug.LogError($"�α��� ����: {ex.Message}");

        } //end catch
    }

    public void Logout()
    {
        _auth.SignOut();
    }

    #region UId�������� �Լ�

    private async Task<string> GetUIdWithServerAsync()
    {
        FirebaseUser user = _auth.CurrentUser;

        if (user == null)
        {
            Debug.LogError("������ ã�� �� �����ϴ�.");

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
