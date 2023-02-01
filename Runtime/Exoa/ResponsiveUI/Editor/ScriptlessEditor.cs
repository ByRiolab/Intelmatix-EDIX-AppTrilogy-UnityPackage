using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Exoa.Responsive
{
    public abstract class ScriptlessEditor : Editor
    {
        protected List<string> _dontIncludeMe = new List<string>() { "m_Script" };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe.ToArray());

            serializedObject.ApplyModifiedProperties();
        }
    }
}
