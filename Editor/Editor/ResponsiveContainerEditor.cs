using UnityEditor;
using UnityEngine;

namespace Intelmatix.Exoa.Responsive
{
    [CustomEditor(typeof(ResponsiveContainer), true)]
    [CanEditMultipleObjects]
    public class ResponsiveContainerEditor : ScriptlessEditor
    {

        public override void OnInspectorGUI()
        {
            ResponsiveContainer r = (ResponsiveContainer)target;
            base.OnInspectorGUI();

            if (r.refreshType != ResponsiveContainer.RefreshType.OnRate && !_dontIncludeMe.Contains("refreshRate"))
            {
                _dontIncludeMe.Add("refreshRate");
            }
            else if (r.refreshType == ResponsiveContainer.RefreshType.OnRate && _dontIncludeMe.Contains("refreshRate"))
            {
                _dontIncludeMe.Remove("refreshRate");
            }
            if (GUILayout.Button("Apply"))
            {
                r.Resize(true);
            }
        }

    }
}
