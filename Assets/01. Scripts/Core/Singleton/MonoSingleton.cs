using UnityEngine;

namespace Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        /// <summary>
        /// �ν��Ͻ�
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

            } //end get

        }

    } //end cs
}