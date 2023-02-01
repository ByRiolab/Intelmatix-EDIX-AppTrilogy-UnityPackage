//====================================================================
// Copyright 2023 exoa.dev
// Special thanks to dtappe on the support forum for his contribution
//====================================================================
using System.Collections.Generic;
using UnityEngine;

using Intelmatix.Exoa.Responsive.Extensions;
namespace Intelmatix.Exoa.Responsive
{
    /// <summary>
    /// The main component for any container containing items.
    /// </summary>
    [ExecuteAlways]
    public class ResponsiveContainer : MonoBehaviour
    {
        [System.Serializable]
        public struct ResponsiveItemData
        {
            public enum Type { Prefered, Percentage, Flexible };
            public Type type;
            [Range(0, 1)]
            public float percentageValue;

            public ResponsiveItemData(Type t) : this()
            {
                this.type = t;
            }

        }
        public RectTransform.Axis axis;

        public enum Behaviour
        {
            None, FitContentInContainer, FitContainerToContent,
            SpreadContentInContainer, SpreadContentInContainerAndExpand,
            GroupContentInContainer, GroupContentInContainerAndExpand
        };
        public Behaviour hBehaviour;
        public Behaviour vBehaviour;
        public enum VAlignType { Top, Middle, Bottom };
        public enum HAlignType { Left, Center, Right };

        public HAlignType hAlign;
        public VAlignType vAlign;
        [SerializeField]
        public RectOffset margins;
        public float spacing;

        private RectTransform container;
        private List<RectTransform> activeChildren;
        private int activeChildCount;
        public ScreenDetector.ScreenConfig.Type restrictTo;

        [Header("REFRESH")]
        public RefreshType refreshType;
        public enum RefreshType { OnChange, OnRate, OnUpdate, Manual };
        public float refreshRate = 1f;
        //public bool allowNestedRefreshing = true;
        private float lastRefresh;
        private float lastRefreshItemsSpace;
        private float lastRefreshContainerSize;

        [Header("DEBUG")]
        public bool debug;
        private float widest = 0;
        private float tallest = 0;

        private bool needsLateResize = false;

        void OnEnable()
        {
            // This object just became active.
            if (Application.isPlaying)
            {
                if (refreshType == RefreshType.OnChange)
                {
                    if (debug) print($"OnEnable: {name}");

                    needsLateResize = true;

                    Resize(true);
                }
            }
        }


        void Start()
        {
            Init();
        }

        /// <summary>
        /// Initialize the component
        /// </summary>
        private void Init()
        {
            container = GetComponent<RectTransform>();
            activeChildren = GetChildrenFirstDepth(container);
            activeChildCount = activeChildren.Count;

            //If parent is a ResponsiveContainer too, disabling the Update, 
            // and let the parent refresh the children containers
            //if (allowNestedRefreshing)
            //{
            //ResponsiveContainer parentRC = transform.parent.GetComponent<ResponsiveContainer>();
            //if (parentRC != null)
            //{
            //refreshType = RefreshType.Manual;
            //}
            //}
        }

        void Update()
        {
            if (refreshType == RefreshType.OnUpdate)
            {
                // Force resize only when not playing
                Resize(!Application.isPlaying);
            }
            else if (refreshType == RefreshType.OnRate)
            {
                if (lastRefresh < Time.time - refreshRate)
                {
                    // Force resize only when not playing
                    Resize(!Application.isPlaying);
                    lastRefresh = Time.time;
                }
            }
            else if (refreshType == RefreshType.OnChange)
            {
                // Detects if children have been resized since last frame
                // or if this container have been resized
                // or if any children have been activated or deactivated
                float newFlexibleSpace = CalculateSpaceBetweenItemsY(true, false,
                    out float totalFlexibleSpaceBetweenItemsPercent,
                    out float minContainerHeightNeeded);

                newFlexibleSpace += CalculateSpaceBetweenItemsX(true, false,
                   out totalFlexibleSpaceBetweenItemsPercent,
                   out minContainerHeightNeeded);

                float newContainerSize = container.GetHeight() + container.GetWidth();

                // detect if active children count has changed since last frame
                List<RectTransform> activeChildren = GetChildrenFirstDepth(container);
                bool activeChildrenCountChanged = activeChildren.Count != activeChildCount;


                if (lastRefreshItemsSpace != newFlexibleSpace ||
                    lastRefreshContainerSize != newContainerSize ||
                    activeChildrenCountChanged ||
                    needsLateResize)
                {
                    Resize(activeChildrenCountChanged || needsLateResize);
                    lastRefreshItemsSpace = newFlexibleSpace;
                    lastRefreshContainerSize = newContainerSize;
                }
            }

        }

        public void LateUpdate()
        {
            if (needsLateResize)
            {
                if (debug) print($"LateUpdate: {name}");

                Resize(true);
                needsLateResize = false;
            }
        }

        /// <summary>
        /// The resize method will handle the rect transform parameters, it's called every frame
        /// </summary>
        /// <param name="forceFullReset"></param>
        public void Resize(bool forceFullReset)
        {

            if (Application.isPlaying && !gameObject.activeInHierarchy)
            {
                if (refreshType == RefreshType.OnChange)
                {
                    needsLateResize = true;
                    if (debug) print($"Resize Delayed: {name}");
                }
                else
                {
                    if (debug) print($"Resize Ignored: {name}");
                }
                return;
            }


            bool isScreenMatching = ScreenDetector.currentConfig.type == restrictTo || restrictTo == ScreenDetector.ScreenConfig.Type.Any;
            if (!isScreenMatching && !forceFullReset)
                return;

            if (margins == null)
                return;

            if (forceFullReset)
            {
                Init();
            }
            float pivotX = 0;
            float pivotY = 0;
            float offsetX = 0;
            float offsetY = 0;
            widest = 0;
            tallest = 0;

            RectTransform rt = null;
            float ch = container.GetHeight();
            float cw = container.GetWidth();
            if ((ch < 10 || cw < 10) && !forceFullReset)
                return;

            if (debug) print("activeChildCount:" + activeChildCount);

            if (axis == RectTransform.Axis.Vertical)
            {
                switch (hAlign)
                {
                    case HAlignType.Left: pivotX = 0; offsetX = margins.left; break;
                    case HAlignType.Center: pivotX = 0.5f; break;
                    case HAlignType.Right: pivotX = 1; offsetX = -margins.right; break;
                }
                if (vBehaviour == Behaviour.GroupContentInContainer || vBehaviour == Behaviour.GroupContentInContainerAndExpand)
                {
                    float flexibleSpaceBetweenEachItemPercent = CalculateSpaceBetweenItemsY(false, true, out float totalFlexibleSpaceBetweenItemsPercent, out float minContainerHeightNeeded);
                    float flexibleMargin = totalFlexibleSpaceBetweenItemsPercent * ch * .5f;
                    float flexibleSpaceBetweenEachItem = spacing;
                    float topOffset = 0;
                    bool contentBiggerThanContainer = flexibleMargin < (margins.top + margins.bottom) * .5f;


                    if (contentBiggerThanContainer)
                    {
                        if (vBehaviour == Behaviour.GroupContentInContainerAndExpand)
                        {
                            flexibleMargin = (margins.top + margins.bottom) * .5f;
                            container.SetHeight(minContainerHeightNeeded);
                            ch = minContainerHeightNeeded;
                        }
                        else
                        {
                            flexibleSpaceBetweenEachItemPercent = CalculateSpaceBetweenItemsY(true, false, out totalFlexibleSpaceBetweenItemsPercent, out minContainerHeightNeeded);
                            flexibleSpaceBetweenEachItem = flexibleSpaceBetweenEachItemPercent * ch;
                        }

                    }
                    else
                    {
                        switch (vAlign)
                        {
                            case VAlignType.Top: flexibleMargin = margins.top; break;
                            case VAlignType.Bottom: flexibleMargin = flexibleMargin * 2 - margins.bottom; break;
                        }
                    }



                    float currentPercent = 1f;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt.gameObject.activeSelf == false) continue;

                        float percent = GetSizePercentage(rt, i, 0, ResponsiveItemData.Type.Prefered);
                        float height = percent * ch;

                        if (!contentBiggerThanContainer)
                        {
                            topOffset = i == 0 ? -flexibleMargin : -spacing;
                        }
                        else
                        {
                            topOffset = i == 0 ? -margins.top : -flexibleSpaceBetweenEachItem;
                        }

                        rt.pivot = new Vector2(pivotX, 1);

                        rt.anchorMax = new Vector2(pivotX, currentPercent);
                        rt.anchorMin = new Vector2(pivotX, currentPercent);
                        currentPercent += -percent + topOffset / ch;

                        rt.anchoredPosition = new Vector2(offsetX, topOffset);

                        widest = CalculateWidth(rt, cw, widest, out float width);

                        rt.sizeDelta = new Vector2(width, height);
                    }
                    if (widest > 0 && hBehaviour == Behaviour.FitContainerToContent)
                        container.SetWidth(widest + margins.left + margins.right);
                }
                else if (vBehaviour == Behaviour.SpreadContentInContainer || vBehaviour == Behaviour.SpreadContentInContainerAndExpand)
                {
                    float flexibleSpaceBetweenEachItemPercent = CalculateSpaceBetweenItemsY(true, false, out float totalFlexibleSpaceBetweenItemsPercent, out float minContainerHeightNeeded);
                    float flexibleSpaceBetweenEachItem = flexibleSpaceBetweenEachItemPercent * ch;
                    float topOffset = 0;

                    if (vBehaviour == Behaviour.SpreadContentInContainerAndExpand && flexibleSpaceBetweenEachItem < spacing)
                    {
                        flexibleSpaceBetweenEachItem = spacing;
                        container.SetHeight(minContainerHeightNeeded);
                        ch = minContainerHeightNeeded;
                    }

                    float currentPercent = 1f;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt.gameObject.activeSelf == false) continue;

                        float percent = GetSizePercentage(rt, i, 0, ResponsiveItemData.Type.Prefered);
                        float height = percent * ch;
                        topOffset = i == 0 ? -margins.top : -flexibleSpaceBetweenEachItem;

                        rt.pivot = new Vector2(pivotX, 1);

                        rt.anchorMax = new Vector2(pivotX, currentPercent);
                        rt.anchorMin = new Vector2(pivotX, currentPercent);
                        currentPercent += -percent + topOffset / ch;

                        rt.anchoredPosition = new Vector2(offsetX, topOffset);

                        widest = CalculateWidth(rt, cw, widest, out float width);

                        rt.sizeDelta = new Vector2(width, height);
                    }

                    if (widest > 0 && hBehaviour == Behaviour.FitContainerToContent)
                        container.SetWidth(widest + margins.left + margins.right);
                }
                else if (vBehaviour == Behaviour.FitContentInContainer)
                {
                    float totalFlexiblePercent = 0;
                    int flexibleItemsCount = 0;
                    float itemFlexibleSpece = CalculateFlexibleItemSpaceY(out totalFlexiblePercent, out flexibleItemsCount);

                    float currentPercent = 1f;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt == null || rt.gameObject.activeSelf == false) continue;

                        float percent = GetSizePercentage(rt, i, itemFlexibleSpece);
                        float height = percent * ch;
                        float topOffset = i == 0 ? -margins.top : -spacing;

                        rt.pivot = new Vector2(pivotX, 1);

                        rt.anchorMax = new Vector2(pivotX, currentPercent);
                        rt.anchorMin = new Vector2(pivotX, currentPercent);
                        currentPercent += -percent + topOffset / ch;

                        rt.anchoredPosition = new Vector2(offsetX, topOffset);

                        widest = CalculateWidth(rt, cw, widest, out float width);


                        rt.sizeDelta = new Vector2(width, height);
                    }
                    if (widest > 0)
                        container.SetWidth(widest + margins.left + margins.right);
                }
                else if (vBehaviour == Behaviour.FitContainerToContent)
                {

                    float topOffset = 0;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt.gameObject.activeSelf == false) continue;

                        float height = rt.GetHeight();

                        topOffset -= i == 0 ? margins.top : spacing;

                        rt.pivot = new Vector2(pivotX, 1);

                        rt.anchorMax = new Vector2(pivotX, 1);
                        rt.anchorMin = new Vector2(pivotX, 1);

                        rt.anchoredPosition = new Vector2(offsetX, topOffset);

                        widest = CalculateWidth(rt, cw, widest, out float width);

                        rt.sizeDelta = new Vector2(width, height);
                        topOffset -= height;

                    }
                    if (debug) print("Height: " + (-topOffset + margins.bottom));
                    container.SetHeight(-topOffset + margins.bottom);
                    if (widest > 0)
                        container.SetWidth(widest + margins.left + margins.right);
                }

            }
            else if (axis == RectTransform.Axis.Horizontal)
            {
                switch (vAlign)
                {
                    case VAlignType.Top: pivotY = 1; offsetY = -margins.top; break;
                    case VAlignType.Middle: pivotY = 0.5f; break;
                    case VAlignType.Bottom: pivotY = 0; offsetY = margins.bottom; break;
                }

                if (hBehaviour == Behaviour.GroupContentInContainer || hBehaviour == Behaviour.GroupContentInContainerAndExpand)
                {
                    float flexibleSpaceBetweenEachItemPercent = CalculateSpaceBetweenItemsX(false, true, out float totalFlexibleSpaceBetweenItemsPercent, out float minContainerWidthNeeded);
                    float flexibleMargin = totalFlexibleSpaceBetweenItemsPercent * cw * .5f;
                    float flexibleSpaceBetweenEachItem = spacing;
                    float leftOffset = 0;
                    bool contentBiggerThanContainer = flexibleMargin < (margins.left + margins.right) * .5f;


                    if (contentBiggerThanContainer)
                    {
                        if (hBehaviour == Behaviour.GroupContentInContainerAndExpand)
                        {
                            flexibleMargin = (margins.left + margins.right) * .5f;
                            container.SetWidth(minContainerWidthNeeded);
                            cw = minContainerWidthNeeded;
                        }
                        else
                        {
                            flexibleSpaceBetweenEachItemPercent = CalculateSpaceBetweenItemsX(true, false, out totalFlexibleSpaceBetweenItemsPercent, out minContainerWidthNeeded);
                            flexibleSpaceBetweenEachItem = flexibleSpaceBetweenEachItemPercent * cw;
                        }

                    }
                    else
                    {
                        switch (hAlign)
                        {
                            case HAlignType.Left: flexibleMargin = margins.left; break;
                            case HAlignType.Right: flexibleMargin = flexibleMargin * 2 - margins.right; break;
                        }
                    }
                    float currentPercent = 0f;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt == null || rt.gameObject == null || rt.gameObject.activeSelf == false) { Init(); continue; }

                        float percent = GetSizePercentage(rt, i, 0, ResponsiveItemData.Type.Prefered);
                        float width = percent * cw;

                        if (!contentBiggerThanContainer)
                        {
                            leftOffset = i == 0 ? flexibleMargin : spacing;
                        }
                        else
                        {
                            leftOffset = i == 0 ? margins.left : flexibleSpaceBetweenEachItem;
                        }

                        rt.pivot = new Vector2(0, pivotY);

                        rt.anchorMax = new Vector2(currentPercent, pivotY);
                        rt.anchorMin = new Vector2(currentPercent, pivotY);
                        currentPercent += percent + leftOffset / cw;

                        rt.anchoredPosition = new Vector2(leftOffset, offsetY);

                        tallest = CalculateHeight(rt, ch, tallest, out float height);

                        rt.sizeDelta = new Vector2(width, height);
                    }
                    if (debug) print("tallest:" + tallest);
                    if (tallest > 0 && vBehaviour == Behaviour.FitContainerToContent)
                        container.SetHeight(tallest + margins.top + margins.bottom);
                }
                else if (hBehaviour == Behaviour.SpreadContentInContainer || hBehaviour == Behaviour.SpreadContentInContainerAndExpand)
                {
                    float flexibleSpaceBetweenEachItemPercent = CalculateSpaceBetweenItemsX(true, false, out float totalFlexibleSpaceBetweenItemsPercent, out float minContainerWidthNeeded);
                    float flexibleSpaceBetweenEachItem = flexibleSpaceBetweenEachItemPercent * cw;
                    float leftOffset = 0;

                    if (hBehaviour == Behaviour.SpreadContentInContainerAndExpand && flexibleSpaceBetweenEachItem < spacing)
                    {
                        flexibleSpaceBetweenEachItem = spacing;
                        container.SetWidth(minContainerWidthNeeded);
                        cw = minContainerWidthNeeded;
                    }
                    float currentPercent = 0f;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt.gameObject.activeSelf == false) continue;

                        float percent = GetSizePercentage(rt, i, 0, ResponsiveItemData.Type.Prefered);
                        float width = percent * cw;
                        leftOffset = i == 0 ? margins.left : flexibleSpaceBetweenEachItem;

                        rt.pivot = new Vector2(0, pivotY);

                        rt.anchorMax = new Vector2(currentPercent, pivotY);
                        rt.anchorMin = new Vector2(currentPercent, pivotY);
                        currentPercent += percent + leftOffset / cw;

                        rt.anchoredPosition = new Vector2(leftOffset, offsetY);

                        tallest = CalculateHeight(rt, ch, tallest, out float height);

                        rt.sizeDelta = new Vector2(width, height);
                    }
                    if (debug) print("tallest:" + tallest);
                    if (tallest > 0 && vBehaviour == Behaviour.FitContainerToContent)
                        container.SetHeight(tallest + margins.top + margins.bottom);
                }
                else if (hBehaviour == Behaviour.FitContentInContainer)
                {
                    float totalFlexiblePercent = 0;
                    int flexibleItemsCount = 0;
                    float itemFlexibleSpece = CalculateFlexibleItemSpaceX(out totalFlexiblePercent, out flexibleItemsCount);

                    float currentPercent = 0f;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt.gameObject.activeSelf == false) continue;

                        float percent = GetSizePercentage(rt, i, itemFlexibleSpece);
                        float width = percent * cw;
                        float leftOffset = i == 0 ? margins.left : spacing;

                        rt.pivot = new Vector2(0, pivotY);

                        rt.anchorMax = new Vector2(currentPercent, pivotY);
                        rt.anchorMin = new Vector2(currentPercent, pivotY);
                        currentPercent += percent + leftOffset / cw;

                        rt.anchoredPosition = new Vector2(leftOffset, offsetY);

                        tallest = CalculateHeight(rt, ch, tallest, out float height);

                        rt.sizeDelta = new Vector2(width, height);
                    }
                    if (tallest > 0)
                        container.SetHeight(tallest + margins.top + margins.bottom);
                }
                else if (hBehaviour == Behaviour.FitContainerToContent)
                {

                    float leftOffset = 0;

                    for (int i = 0; i < activeChildCount; i++)
                    {
                        rt = activeChildren[i] as RectTransform;
                        if (rt.gameObject.activeSelf == false) continue;

                        float width = rt.GetWidth();

                        leftOffset += i == 0 ? margins.left : spacing;

                        rt.pivot = new Vector2(0, pivotY);

                        rt.anchorMax = new Vector2(0, pivotY);
                        rt.anchorMin = new Vector2(0, pivotY);

                        rt.anchoredPosition = new Vector2(leftOffset, offsetY);

                        tallest = CalculateHeight(rt, ch, tallest, out float height);


                        rt.sizeDelta = new Vector2(width, height);
                        leftOffset += width;

                    }
                    container.SetWidth(Mathf.Abs(leftOffset + margins.right));
                    if (debug) print("tallest:" + tallest);
                    if (tallest > 0)
                        container.SetHeight(tallest + margins.top + margins.bottom);
                }
            }

            if (debug) print("Resized:" + gameObject.name + " activeChildCount:" + activeChildCount + " forceFullReset:" + forceFullReset);

            for (int i = 0; i < activeChildCount; i++)
            {
                rt = activeChildren[i] as RectTransform;
                var rc = rt.GetComponent<ResponsiveContainer>();
                if (rc != null)
                    rc.Resize(forceFullReset);
            }
            //}

        }

        #region UTILS
        /// <summary>
        /// Calculates the height of a container, regarding his behaviour settings
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="ch"></param>
        /// <param name="tallest"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private float CalculateHeight(RectTransform rt, float ch, float tallest, out float height)
        {
            height = rt.sizeDelta.y;
            if (vBehaviour == Behaviour.FitContentInContainer)
            {
                height = ch - margins.top - margins.bottom;
            }
            else if (vBehaviour == Behaviour.FitContainerToContent)
            {
                if (height > tallest)
                    tallest = height;
            }
            return tallest;
        }


        /// <summary>
        /// Calculates the width of a container regarding his behaviour settings
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="cw"></param>
        /// <param name="widest"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private float CalculateWidth(RectTransform rt, float cw, float widest, out float width)
        {
            width = rt.sizeDelta.x;

            if (hBehaviour == Behaviour.FitContentInContainer)
            {
                width = cw - margins.right - margins.left;
            }
            else if (hBehaviour == Behaviour.FitContainerToContent)
            {
                if (width > widest)
                    widest = width;
            }
            return widest;
        }

        /// <summary>
        /// Calculate space betwwen items taking into account all items and margins.
        /// </summary>
        /// <param name="includeMargins"></param>
        /// <param name="includeSpacing"></param>
        /// <param name="totalFlexiblePercent"></param>
        /// <param name="minContainerHeightNeeded"></param>
        /// <returns></returns>
        private float CalculateSpaceBetweenItemsY(bool includeMargins, bool includeSpacing, out float totalFlexiblePercent, out float minContainerHeightNeeded)
        {
            float remainingPercent = 1f;
            float ch = container.GetHeight();
            if (includeMargins)
            {
                remainingPercent -= margins.top / ch;
                remainingPercent -= margins.bottom / ch;
            }
            if (includeSpacing)
            {
                remainingPercent -= (spacing * (activeChildCount - 1)) / ch;
            }
            minContainerHeightNeeded = margins.top + margins.bottom + spacing * (activeChildCount - 1);

            RectTransform rt = null;
            for (int i = 0; i < activeChildCount; i++)
            {
                rt = activeChildren[i] as RectTransform;
                if (rt == null || rt.gameObject == null || rt.gameObject.activeSelf == false) { Init(); continue; }

                float percent = GetSizePercentage(rt, i, 0, ResponsiveItemData.Type.Prefered);
                remainingPercent -= percent;
                minContainerHeightNeeded += rt.GetHeight();
            }
            totalFlexiblePercent = remainingPercent;
            return totalFlexiblePercent / (activeChildCount - 1);
        }

        /// <summary>
        /// Calculate space betwwen items taking into account all items and margins.
        /// </summary>
        /// <param name="includeMargins"></param>
        /// <param name="includeSpacing"></param>
        /// <param name="totalFlexiblePercent"></param>
        /// <param name="minContainerWidthNeeded"></param>
        /// <returns></returns>
        private float CalculateSpaceBetweenItemsX(bool includeMargins, bool includeSpacing, out float totalFlexiblePercent, out float minContainerWidthNeeded)
        {
            float remainingPercent = 1f;
            float cw = container.GetWidth();
            if (includeMargins)
            {
                remainingPercent -= margins.left / cw;
                remainingPercent -= margins.right / cw;
            }
            if (includeSpacing)
            {
                remainingPercent -= (spacing * (activeChildCount - 1)) / cw;
            }
            minContainerWidthNeeded = margins.left + margins.right + spacing * (activeChildCount - 1);

            RectTransform rt = null;
            for (int i = 0; i < activeChildCount; i++)
            {
                rt = activeChildren[i] as RectTransform;
                if (rt == null || rt.gameObject == null || rt.gameObject.activeSelf == false) { Init(); continue; }

                float percent = GetSizePercentage(rt, i, 0, ResponsiveItemData.Type.Prefered);
                remainingPercent -= percent;
                minContainerWidthNeeded += rt.GetWidth();
            }
            totalFlexiblePercent = remainingPercent;
            return totalFlexiblePercent / (activeChildCount - 1);
        }

        /// <summary>
        /// Calculates the remaining space after all object fixed sized have been taken into account.
        /// </summary>
        /// <param name="totalFlexiblePercent"></param>
        /// <param name="flexibleItemsCount"></param>
        /// <returns></returns>
        private float CalculateFlexibleItemSpaceX(out float totalFlexiblePercent, out int flexibleItemsCount)
        {
            float remainingPercent = 1f;
            float cw = container.GetWidth();
            cw = cw == 0 ? 10 : cw;
            remainingPercent -= margins.left / cw;
            remainingPercent -= margins.right / cw;
            remainingPercent -= (activeChildCount - 1) * spacing / cw;
            RectTransform rt = null;
            int count = 0;
            for (int i = 0; i < activeChildCount; i++)
            {
                rt = activeChildren[i] as RectTransform;
                if (rt == null || rt.gameObject == null || rt.gameObject.activeSelf == false) { Init(); continue; }

                float percent = GetSizePercentage(rt, i, 0);
                remainingPercent -= percent;
                if (percent == 0) count++;
            }
            totalFlexiblePercent = remainingPercent;
            flexibleItemsCount = count == 0 ? 1 : count;
            return totalFlexiblePercent / flexibleItemsCount;
        }


        /// <summary>
        /// Calculates the remaining space after all object fixed sized have been taken into account.
        /// </summary>
        /// <param name="totalFlexiblePercent"></param>
        /// <param name="flexibleItemsCount"></param>
        /// <returns></returns>
        private float CalculateFlexibleItemSpaceY(out float totalFlexiblePercent, out int flexibleItemsCount)
        {
            float remainingPercent = 1f;
            float ch = container.GetHeight();
            ch = ch == 0 ? 10 : ch;
            remainingPercent -= margins.top / ch;
            remainingPercent -= margins.bottom / ch;
            remainingPercent -= (activeChildCount - 1) * spacing / ch;
            RectTransform rt = null;
            int count = 0;
            for (int i = 0; i < activeChildCount; i++)
            {
                rt = activeChildren[i] as RectTransform;
                if (rt == null || rt.gameObject == null || rt.gameObject.activeSelf == false) { Init(); continue; }

                float percent = GetSizePercentage(rt, i, 0);
                remainingPercent -= percent;
                if (percent == 0) count++;
            }
            totalFlexiblePercent = remainingPercent;
            flexibleItemsCount = count == 0 ? 1 : count;
            return totalFlexiblePercent / flexibleItemsCount;
        }

        /// <summary>
        /// Converts a rect transform size into a percentage value
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="i"></param>
        /// <param name="flexibleSpace"></param>
        /// <param name="defaultSizeType"></param>
        /// <returns></returns>

        private float GetSizePercentage(RectTransform rt, int i, float flexibleSpace, ResponsiveItemData.Type defaultSizeType = ResponsiveItemData.Type.Flexible)
        {
            ResponsiveItem riComp = activeChildren[i].GetComponent<ResponsiveItem>();
            ResponsiveItemData ri = riComp != null ? riComp.settings : new ResponsiveItemData(defaultSizeType);

            if (ri.type == ResponsiveItemData.Type.Prefered)
            {
                return axis == RectTransform.Axis.Vertical ? rt.GetHeight() / container.GetHeight() :
                  rt.GetWidth() / container.GetWidth();
            }
            else if (ri.type == ResponsiveItemData.Type.Percentage)
            {
                return Mathf.Clamp01(ri.percentageValue);
            }
            else if (ri.type == ResponsiveItemData.Type.Flexible)
            {
                return flexibleSpace;
            }
            return 0;
        }


        public List<RectTransform> GetChildrenFirstDepth(RectTransform container)
        {
            List<RectTransform> list = new List<RectTransform>();
            for (int i = 0; i < container.childCount; i++)
            {
                RectTransform child = container.GetChild(i) as RectTransform;
                if (child != null && child.gameObject != null && child.gameObject.activeSelf)
                    list.Add(child);
            }
            return list;
        }
        #endregion
    }

}
