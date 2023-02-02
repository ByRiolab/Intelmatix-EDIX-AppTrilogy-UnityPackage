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
                    Debug.LogFormat("No instance of type {0} found", typeof(T).Name);
                    return null;
                }
                return instance;
            }
        }

        void Awake()
        {
            if (instance == null && dataReference != null)
            {
                instance = this as T;
                // Debug.LogFormat("Singleton {0} Initialized", typeof(T).Name);
            }
            else if (dataReference == null)
            {
                Debug.LogErrorFormat("Singleton {0} data reference is null", typeof(T).Name);
                DestroyImmediate(gameObject);
            }
            else
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
                    Debug.LogFormat("No instance of type {0} found", typeof(T).Name);
                    return null;
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
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }

}
