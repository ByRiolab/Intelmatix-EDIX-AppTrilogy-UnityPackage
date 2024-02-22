using System;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-1)]
public class ScrollHighlight : MonoBehaviour
{

    enum ScrollPositionReference
    {
        CENTER, TOP, BOTTOM
    }

    [SerializeField] private ScrollRect scrollRect;
    private RectTransform Content => scrollRect.content;

    [SerializeField] private ScrollPositionReference positionReference;

    [SerializeField] private byte indexing = 0;
    [SerializeField] private float maxPositionDiff = 1500;
    [SerializeField] private float offset = 0;
    [Space]
    [SerializeField] private Vector2 alphaMinMax = new(0.01f, 1);
    [SerializeField] private uint alphaExp = 12;
    [Space]
    [SerializeField] private Vector2 scaleMinMax = new(0.01f, 1.2f);
    [SerializeField] private uint scaleExp = 8;

    private void LateUpdate()
    {
        for (int i = 0; i < Content.childCount; i++)
        {
            var item = Content.GetChild(i);
            for (int j = 0; j < indexing; j++)
            {
                if (item.childCount > 0)
                {
                    item = item.GetChild(0);
                }
            }

            if (!item.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup = item.gameObject.AddComponent<CanvasGroup>();
            }


            float viewportPosition = scrollRect.viewport.position.y + offset;
            if (positionReference == ScrollPositionReference.TOP)
            {
                viewportPosition += scrollRect.viewport.rect.height / 4;
            }
            else if (positionReference == ScrollPositionReference.BOTTOM)
            {
                viewportPosition -= scrollRect.viewport.rect.height / 4;
            }

            float diff = Mathf.Abs(item.position.y - viewportPosition);


            float delta = Mathf.InverseLerp(maxPositionDiff, 0, diff);

            float alpha = Mathf.Lerp(alphaMinMax.x, alphaMinMax.y, Mathf.Pow(delta, alphaExp));
            canvasGroup.alpha = alpha;

            float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, Mathf.Pow(delta, scaleExp));
            item.transform.localScale = new(scale, scale, 1);
        }
    }
}
