using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Intelmatix.Modules.Shared.Helpers
{
    public class TextVerticalSizeAdjuster : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;
        public RectTransform rectTransform;
        public float maxHieght;
        public float minHieght;

        public void UpdateSize()
        {
            Vector2 preferredValues = textMeshPro.GetPreferredValues();
            float height = Mathf.Min(preferredValues.y, maxHieght);
            height = Mathf.Max(height, minHieght);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}