using UnityEditor;
using UnityEngine;

namespace Intelmatix.Exoa.Responsive
{
    [CustomEditor(typeof(Resizer), true)]
    [CanEditMultipleObjects]
    public class ResizerEditor : ScriptlessEditor
    {

        public override void OnInspectorGUI()
        {
            Resizer r = (Resizer)target;
            base.OnInspectorGUI();


            if (GUILayout.Button("Apply"))
            {
                r.Resize(true);
            }
        }

    }
}
