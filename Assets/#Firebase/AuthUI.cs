using Firebase;
using ShardData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthUI : MonoBehaviour
{
    public TMP_InputField EmailInput = null;
    public TMP_InputField PasswordInput = null;

    private void Start()
    {
        FirebaseApp app;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                Debug.Log("버전 : " + app.Options.DatabaseUrl);
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    public void OnCreate()
    {
        AuthManager.Instance.Create(EmailInput.text, PasswordInput.text);
    }

    public async void OnUpdateUserData()
    {
        UserServerData prevData = await UserServerDataManager.Instance.GetUserServerDataWithServerAsync();

        UserServerData data = new()
        {
            Coin = prevData.Coin + 1000,
            PlayerId = prevData.PlayerId,
            SkinList = prevData.SkinList,
            UserName = "성호",
        };

        await UserServerDataManager.Instance.UpdateUserServerDataWithServerAsync(data);
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
