using System;
using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-1)]
public class ScrollHighlight : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform Content => scrollRect.content;

    [SerializeField] private Vector2 alphaMinMax = new(0.01f, 1);
    [SerializeField] private uint alphaExp = 12;
    [SerializeField] private Vector2 scaleMinMax = new(0.01f, 1.2f);
    [SerializeField] private uint scaleExp = 8;

    private void LateUpdate()
    {
        for (int i = 0; i < Content.childCount; i++)
        {
            var item = Content.GetChild(i);

            if (!item.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup = item.gameObject.AddComponent<CanvasGroup>();
            }

            float position = 1 - ((float)i) / Content.childCount;


            float delta = 1 - Mathf.Abs(scrollRect.normalizedPosition.y - position);

            float alpha = Mathf.Lerp(alphaMinMax.x, alphaMinMax.y, Mathf.Pow(delta, alphaExp));
            canvasGroup.alpha = alpha;

            float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, Mathf.Pow(delta, scaleExp));
            item.transform.localScale = new(scale, scale, 1);
        }
    }
}
