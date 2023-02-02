using System.Collections.Generic;
using Intelmatix.Exoa.Responsive.Extensions;
using UnityEngine;
namespace Intelmatix.Exoa.Responsive
{
    /// A class that detects the screen orientation and sends
    /// feedbacks to other components
    [ExecuteAlways]
    public class ScreenDetector : MonoBehaviour
    {
        private RectTransform rt;
        private RectTransform canvas;
        private float canvasWidth;
        private float canvasHeight;


        [System.Serializable]
        public struct ScreenConfig
        {
            public enum Type { Any, Portrait, Landscape };
            public Type type;
            public float minWidth;
            public float maxWidth;
        }
        public delegate void OnScreenConfigChangeHandler(ScreenConfig conf);
        public static OnScreenConfigChangeHandler OnScreenConfigChange;

        public static ScreenConfig currentConfig;

        void Start()
        {
            rt = GetComponent<RectTransform>();
            canvas = rt.root.GetComponentInChildren<RectTransform>();

            EvaluateScreenConfig(false);
        }

        void Update()
        {
            if (canvasWidth != canvas.GetWidth() || canvasHeight != canvas.GetHeight())
            {
                EvaluateScreenConfig(true);
            }
        }

        private void EvaluateScreenConfig(bool sendEvent)
        {
            canvasWidth = canvas.GetWidth();
            canvasHeight = canvas.GetHeight();
            if (canvasWidth > canvasHeight && currentConfig.type != ScreenConfig.Type.Landscape)
            {
                currentConfig.type = ScreenConfig.Type.Landscape;
                if (sendEvent)
                    OnScreenConfigChange?.Invoke(currentConfig);
            }
            else if (canvasWidth < canvasHeight && currentConfig.type != ScreenConfig.Type.Portrait)
            {
                currentConfig.type = ScreenConfig.Type.Portrait;
                if (sendEvent)
                    OnScreenConfigChange?.Invoke(currentConfig);
            }
        }

    }
}
