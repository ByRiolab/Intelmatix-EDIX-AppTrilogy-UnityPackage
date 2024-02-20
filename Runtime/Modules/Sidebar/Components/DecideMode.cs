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
        public void Display(UnityEngine.Events.UnityAction humanMonde, UnityEngine.Events.UnityAction cognitiveMode)
        {
            humanButton.onValueChanged.AddListener((value) =>
            {
                if (value)
                {

                    humanMonde?.Invoke();
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
                AnimationManager.AnimateIn(this.canvasGroup, direction: AnimationManager.Direction.Right);
            }
        }

        private void OnDisable()
        {
            if (canvasGroup != null)
            {
                AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Right);
            }
        }

        public LTDescr Hide()
        {
            return AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Right);
        }

        private IEnumerator ChangePosition(bool top)
        {
            const float speed = 1500;
            bool condition()
            {
                if (top)
                {
                    return ((RectTransform)transform).anchoredPosition.y < 0;
                }
                else
                {
                    return ((RectTransform)transform).anchoredPosition.y > -((RectTransform)transform.parent).rect.height + ((RectTransform)transform).rect.height;
                }
            }
            while (condition())
            {
                ((RectTransform)transform).anchoredPosition += new Vector2(0, (top ? speed : -speed) * Time.deltaTime);
                yield return null;
            }
        }


    }
}
