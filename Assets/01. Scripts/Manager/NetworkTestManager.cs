using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkTestManager : MonoBehaviour
{
    public string SceneName = "";
    [SerializeField] private TMP_InputField _inputField;

    public async void CreateRoom()
    {
        await HostSingle.Instance.GameManager.StartHostAsync(Guid.NewGuid().ToString(), AppController.Instance.GetUserData("이름없음"));

        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public async void JoinRoom()
    {

        await ClientSingle.Instance.GameManager.StartClientAsync(_inputField.text, AppController.Instance.GetUserData("이름없음"));

        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {

            //NetworkGameManager.Instance.StartGame();

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            CreateRoom();

        }

        if (Input.GetKeyDown(KeyCode.X))
        {

            JoinRoom();

        }

    }
}
