using UnityEngine;
using UnityEngine.UI;

using Intelmatix.Exoa.Responsive.Extensions;
namespace Intelmatix.Exoa.Responsive
{
    /// <summary>
    /// An independent component to resize an rect transform itself
    /// </summary>
    [ExecuteAlways]
    public class Resizer : MonoBehaviour
    {
        private Image img;
        private RawImage rimg;
        private RectTransform rt;
        private RectTransform container;
        private float containerWidth;
        private float containerHeight;

        public enum ResizeMode { Fill, Cover, Contain };
        public enum VAlign { Top, Middle, Bottom };
        public enum HAlign { Left, Center, Right };

        public ResizeMode resizeMode;
        public VAlign vAlign;
        public HAlign hAlign;
        private float ratio;

        [Header("Limits For Filling Mode")]
        public bool enableWdithLimits;
        public float minWidth;
        public float maxWidth;

        public bool enableHeightLimits;
        public float minHeight;
        public float maxHeight;

        [Header("Margins")]
        public RectOffset margins;

        [Header("Ratio")]
        public bool useFixedRatio;
        public float fixedRatio = 0.73586405125f;
        public bool useTextureRatio;

        void Start()
        {
            Init();
        }

        private void Init()
        {
            img = GetComponent<Image>();
            rimg = GetComponent<RawImage>();
            rt = GetComponent<RectTransform>();
            container = rt.parent.GetComponent<RectTransform>();

            ratio = rt.GetWidth() / rt.GetHeight();
            if (useFixedRatio) ratio = fixedRatio;
            else if (useTextureRatio && img != null && img.sprite != null) ratio = img.sprite.bounds.size.x / img.sprite.bounds.size.y;
            else if (useTextureRatio && rimg != null && rimg.texture != null) ratio = rimg.texture.width / rimg.texture.height;
        }

        public void Update()
        {
            Resize();
        }
        /// <summary>
        /// The main method that resizes the rect transform
        /// </summary>
        /// <param name="force"></param>
        public void Resize(bool force = false)
        {
            if (container == null || force) Init();

            if (ratio <= 0 || float.IsNaN(ratio))
                return;

            if (containerWidth != container.GetWidth() || containerHeight != container.GetHeight() || force)
            {
                containerWidth = container.GetWidth();
                containerHeight = container.GetHeight();

                float containerWidthMargin = containerWidth;
                float containerHeightMargin = containerHeight;
                bool outerMargin = false;

                if (margins == null)
                    margins = new RectOffset();

                switch (resizeMode)
                {
                    case ResizeMode.Cover:
                        containerWidthMargin = containerWidth + margins.left + margins.right;
                        containerHeightMargin = containerHeight + margins.top + margins.bottom;
                        outerMargin = true;
                        break;
                    case ResizeMode.Fill:
                    case ResizeMode.Contain:
                        containerWidthMargin = containerWidth - margins.left - margins.right;
                        containerHeightMargin = containerHeight - margins.top - margins.bottom;
                        break;
                }
                float height = containerWidthMargin / ratio;
                float width = containerHeightMargin * ratio;

                float ax = 0f, ay = 0f, px = 0f, py = 0f;
                switch (vAlign)
                {
                    case VAlign.Top: ay = 1; py = -margins.top; break;
                    case VAlign.Middle: ay = .5f; py = 0; break;
                    case VAlign.Bottom: ay = 0; py = margins.bottom; break;
                }
                switch (hAlign)
                {
                    case HAlign.Right: ax = 1; px = -margins.right; break;
                    case HAlign.Center: ax = .5f; px = 0; break;
                    case HAlign.Left: ax = 0; px = margins.left; break;
                }
                if (outerMargin)
                {
                    px *= -1;
                    py *= -1;
                }

                rt.pivot = rt.anchorMin = rt.anchorMax = new Vector2(ax, ay);
                rt.anchoredPosition = new Vector2(px, py);


                if (resizeMode == ResizeMode.Fill)
                {
                    float finalWidth = containerWidthMargin;
                    float finalHeight = containerHeightMargin;

                    if (enableWdithLimits)
                        finalWidth = Mathf.Clamp(finalWidth, minWidth, maxWidth);

                    if (enableHeightLimits)
                        finalHeight = Mathf.Clamp(finalHeight, minHeight, maxHeight);

                    rt.SetWidth(finalWidth);
                    rt.SetHeight(finalHeight);
                }
                else if (resizeMode == ResizeMode.Cover)
                {
                    if (height < containerHeightMargin)
                    {
                        rt.SetWidth(width);
                        rt.SetHeight(containerHeightMargin);
                    }
                    else
                    {
                        rt.SetWidth(containerWidthMargin);
                        rt.SetHeight(height);
                    }
                }
                else if (resizeMode == ResizeMode.Contain)
                {
                    if (height > containerHeightMargin)
                    {
                        rt.SetWidth(width);
                        rt.SetHeight(containerHeightMargin);
                    }
                    else
                    {
                        rt.SetWidth(containerWidthMargin);
                        rt.SetHeight(height);
                    }
                }
            }
        }
    }
}
