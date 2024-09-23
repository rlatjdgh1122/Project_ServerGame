using UnityEngine;

public class MonoSingleton<T> : ExpansionMonoBehaviour where T : MonoBehaviour
{
    public bool IsDontDestroyLoad = false;

    private static T _instance;

    /// <summary>
    /// 인스턴스
    /// </summary>
    public static T Instance
    {

        get
        {

            if (_instance == null)
            {

                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {

                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();

                }

            }

            return _instance;

        }

    }

    public virtual void Awake()
    {
        if (IsDontDestroyLoad)
        {
            if (GameObject.FindObjectOfType<T>() != null)
            {
                DontDestroyOnLoad(gameObject);
            }

        } //end if
    }

}