using UnityEngine;

namespace Helpers
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;
        public bool isPersistant;

        private void Awake()
        {
            if (isPersistant)
            {
                if (_instance)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _instance = this as T;
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                _instance = this as T;
            }
        }
    }
}
