// Simple Scroll-Snap - https://assetstore.unity.com/packages/tools/gui/simple-scroll-snap-140884
// Copyright (c) Daniel Lochner

using UnityEngine;

namespace DanielLochner.Assets.SimpleScrollSnap
{
    public class Scale : TransitionEffectBase<RectTransform>
    {
        [SerializeField] private byte index = 0;
        public override void OnTransition(RectTransform rectTransform, float scale)
        {
            while (index > 0)
            {
                if (rectTransform.childCount > 0)
                    rectTransform = (RectTransform)rectTransform.GetChild(0);
                index--;
            }
            rectTransform.localScale = Vector3.one * scale;
        }
    }
}