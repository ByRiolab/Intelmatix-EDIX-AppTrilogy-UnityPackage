
using System.IO;
using UnityEngine;

namespace Intelmatix.Structure
{
    /// <summary>
    /// <description>
    /// This class is a generic class that can be used to create a variable of any type
    /// It is used to create a scriptable object that can be used to create a variable of any type
    /// </description><br/>
    /// @author:  Anthony Shinomiya M.
    /// @date:  2022-01-21
    /// </summary>
    public class DataReference<T> : ScriptableObject
    {
        [Tooltip("The value of the variable")]
        [SerializeField] private T data;

        [System.NonSerialized] private bool isInitialized = false;

        public bool IsInitialized
        {
            get { return isInitialized; }
        }
        public T Data
        {
            get { return data; }
            set
            {
                data = value;
                _onDataChanged?.Invoke(data);
                isInitialized = true;
            }
        }

        //On change event
        public delegate void OnDataChangedEvent(T data);
        private event OnDataChangedEvent _onDataChanged;

        public event OnDataChangedEvent OnDataChanged
        {
            add
            {
                _onDataChanged += value;
                if (isInitialized)
                    value.Invoke(data);
            }
            remove
            {
                _onDataChanged -= value;
            }
        }


        //Clean the listeners
        public void CleanListeners()
        {
            _onDataChanged = null;
        }

        public void SetData(T data)
        {
            this.data = data;
            _onDataChanged?.Invoke(data);
            isInitialized = true;
        }

    }

}