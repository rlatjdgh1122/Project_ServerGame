using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNameEnum
{
    None = 0,

}

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(SceneNameEnum type)
    {
        SceneManager.LoadScene(type.ToString());
    }

    public void LoadSceneByNetwork(string sceneName)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadSceneByNetwork(SceneNameEnum type)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(type.ToString(), LoadSceneMode.Single);
    }
}
