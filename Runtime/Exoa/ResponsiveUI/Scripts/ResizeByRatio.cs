using Exoa.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Exoa.Responsive
{
    /// <summary>
    ///  a very simple component to resize a rect transform based on
    ///  a specific ratio or image ratio
    /// </summary>
    [ExecuteAlways]
    public class ResizeByRatio : MonoBehaviour
    {
        public float ratio = 0.73586405125f;
        public enum Type { ResizeWidth, ResizeHeight };
        public Type type;
        private RectTransform rt;
        private Image img;
        private RawImage rimg;
        public bool useImageTextureSize;

        void Start()
        {
            Init();
        }

        private void Init()
        {
            rt = GetComponent<RectTransform>();
            img = GetComponent<Image>();
            rimg = GetComponent<RawImage>();

            if (useImageTextureSize && img != null) ratio = img.sprite.bounds.size.x / img.sprite.bounds.size.y;
            else if (useImageTextureSize && rimg != null)
            {
                ratio = (float)rimg.texture.width / (float)rimg.texture.height;
                print(" rimg.texture.width:" + rimg.texture.width + "  rimg.texture.height:" + rimg.texture.height + " ratio:" + ratio);
            }
        }

        void Update()
        {
            Resize(false);
        }
        public void Resize(bool force)
        {
            if (force) Init();

            if (ratio <= 0 || float.IsNaN(ratio))
                return;

            float w = rt.GetWidth();
            float h = rt.GetHeight();
            float newRatio = w / h;
            if (newRatio != ratio || force)
            {
                if (type == Type.ResizeHeight)
                {
                    h = w / ratio;
                    rt.SetHeight(h);
                }
                else if (type == Type.ResizeWidth)
                {
                    w = h * ratio;
                    rt.SetWidth(w);
                }
            }
        }
    }
}
