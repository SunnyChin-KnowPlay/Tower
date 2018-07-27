using UnityEngine;

namespace Dream.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static T s_Instance;
        protected bool isDontDestroy = false;

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
                        s_Instance.isDontDestroy = true;
                        DontDestroyOnLoad(s_Instance.gameObject);
                    }
                    else
                    {
                        s_Instance = obj.GetComponent<T>();
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
                    s_Instance.isDontDestroy = true;
                    DontDestroyOnLoad(s_Instance.gameObject);
                }
                else
                {
                    s_Instance = obj.GetComponent<T>();
                }
            }
            return s_Instance;
        }

        protected virtual void OnDestroy()
        {
            if (s_Instance && !s_Instance.isDontDestroy)
                s_Instance = null;
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
