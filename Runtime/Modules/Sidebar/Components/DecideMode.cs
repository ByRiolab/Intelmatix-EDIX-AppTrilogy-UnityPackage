using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Intelmatix.Modules.Sidebar
{
    public class DecideMode : MonoBehaviour
    {
        [Header("Toggle")]
        [SerializeField] private ToggleGroup toggleGroup;
        [Space]
        [SerializeField] private CanvasGroup humanCanvasGroup;
        [SerializeField] private Toggle humanButton;
        [Space]
        [SerializeField] private CanvasGroup cognitiveCanvasGroup;
        [SerializeField] private Toggle cognitiveButton;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasGroup;
        // [SerializeField] private Button button;


        private void Awake()
        {
            var alpha = 0.25f;
            var duration = 0.5f;
            humanCanvasGroup.alpha = alpha;
            cognitiveCanvasGroup.alpha = alpha;

            humanButton.onValueChanged.AddListener((value) =>
            {
                StartCoroutine(ChangePosition(true));
                humanCanvasGroup.LeanAlpha(value ? 1 : alpha, duration);
            });
            cognitiveButton.onValueChanged.AddListener((value) =>
            {
                StartCoroutine(ChangePosition(false));
                cognitiveCanvasGroup.LeanAlpha(value ? 1 : alpha, duration);
            });
            StartCoroutine(ChangePosition(false));
        }
        public void Display(UnityAction humanMode, UnityAction cognitiveMode)
        {
            humanButton.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    humanMode?.Invoke();
                    SidebarManager.Instance.RestoreKPI();
                }
            });
            cognitiveButton.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    cognitiveMode?.Invoke();
                }
            });
        }

        private void OnEnable()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                AnimationManager.AnimateIn(this.canvasGroup, direction: AnimationManager.Direction.Up);
            }
        }

        private void OnDisable()
        {
            if (canvasGroup != null)
            {
                AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Down);
            }
        }

        public LTDescr Hide()
        {
            return AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Down);
        }

        private IEnumerator ChangePosition(bool top)
        {
            const float duration = 1f; // Total duration for movement
            float elapsed = 0f; // Time elapsed

            Vector2 startPos = ((RectTransform)transform).anchoredPosition;
            Vector2 endPos;

            if (top)
            {
                endPos = new Vector2(startPos.x, 0);
            }
            else
            {
                endPos = new Vector2(startPos.x, -((RectTransform)transform.parent).rect.height + ((RectTransform)transform).rect.height);
            }

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float factor = EaseOutCubic(t);

                ((RectTransform)transform).anchoredPosition = Vector2.Lerp(startPos, endPos, factor);

                elapsed += Time.deltaTime;
                yield return null;
            }

     // Ensure final position
     ((RectTransform)transform).anchoredPosition = endPos;
        }

        private float EaseOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }


    }
}
