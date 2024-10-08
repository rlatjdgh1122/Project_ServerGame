using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkTestManager : MonoBehaviour
{
    public string SceneName = "";
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_InputField  _inputName;

    public async void CreateRoom()
    {
        await HostSingle.Instance.GameManager.StartHostAsync(Guid.NewGuid().ToString(), AppController.Instance.GetUserData($"{_inputName.text}"));

        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public async void JoinRoom()
    {

        await ClientSingle.Instance.GameManager.StartClientAsync(_inputField.text, AppController.Instance.GetUserData($"{_inputName.text}"));

        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

}
