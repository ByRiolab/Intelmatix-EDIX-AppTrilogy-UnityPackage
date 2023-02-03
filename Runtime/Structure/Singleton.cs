using UnityEngine;

namespace Intelmatix.Base
{
    public class Singleton<T, U> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Data References"), SerializeField]
        protected U dataReference; // is Required
        [Space(20)]

        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                    }

                }
                return instance;
            }
        }


        void Awake()
        {
            if (instance == null && dataReference != null)
            {
                instance = this as T;
            }
            else if (instance != null && dataReference != null)
            {
                Debug.LogErrorFormat("Singleton {0} already initialized", typeof(T).Name);
                DestroyImmediate(gameObject);
            }
        }

    }


    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                    }

                }
                return instance;
            }
        }


        void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                // Debug.LogFormat("Singleton {0} Initialized", typeof(T).Name);
            }
            else if (instance != this)
            {
                Debug.LogErrorFormat("Singleton {0} already initialized", typeof(T).Name);
                DestroyImmediate(gameObject);
            }
        }
    }

}
