using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-1)]
public class ScrollHighlight : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform Content => scrollRect.content;
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
            float delta = 1 - Mathf.Abs(scrollRect.verticalNormalizedPosition - position);
            var alpha = Mathf.Lerp(0.01f, 1, delta * delta);
            canvasGroup.alpha = alpha;
            item.transform.localScale = new(alpha, alpha, 1);
        }
    }
}
