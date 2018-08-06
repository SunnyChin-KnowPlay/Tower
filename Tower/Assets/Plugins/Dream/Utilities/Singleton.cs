using UnityEngine;

namespace Dream.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T s_Instance;

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    var obj = GameObject.FindObjectOfType(typeof(T)) as T;

                    if (obj == null)
                    {
                        var go = new GameObject("Singleton of " + typeof(T).ToString());
                        s_Instance = go.AddComponent<T>();
                    }
                    else
                    {
                        s_Instance = obj.GetComponent<T>();
                    }

                    if (null != s_Instance)
                    {
                        DontDestroyOnLoad(s_Instance.gameObject);
                    }
                }
                return s_Instance;
            }
        }

        public static T GetInstance()
        {
            if (s_Instance == null)
            {
                var obj = GameObject.FindObjectOfType(typeof(T)) as T;

                if (obj == null)
                {
                    var go = new GameObject("Singleton of " + typeof(T).ToString());
                    s_Instance = go.AddComponent<T>();
                }
                else
                {
                    s_Instance = obj.GetComponent<T>();
                }

                if (null != s_Instance)
                {
                    DontDestroyOnLoad(s_Instance.gameObject);
                }
            }
            return s_Instance;
        }

        protected virtual void OnDestroy()
        {

        }

        public virtual void DestroySingleton()
        {
            if (null != s_Instance)
            {
                Destroy(s_Instance.gameObject);
                s_Instance = null;
            }
        }
    }
}
