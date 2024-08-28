using UnityEditor.Build.Content;
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

}
