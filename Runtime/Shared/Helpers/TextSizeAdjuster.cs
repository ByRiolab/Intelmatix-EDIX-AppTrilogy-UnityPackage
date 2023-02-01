using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Intelmatix.Modules.Shared.Helpers
{
    public class TextSizeAdjuster : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;
        public RectTransform rectTransform;
        public float maxWidth;
        public float margin;

        private void Start()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateSize()
        {
            Vector2 preferredValues = textMeshPro.GetPreferredValues();
            float width = Mathf.Min(preferredValues.x, maxWidth);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + margin);
        }
    }
}