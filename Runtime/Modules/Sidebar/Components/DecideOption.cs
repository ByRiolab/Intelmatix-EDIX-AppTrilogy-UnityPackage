using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Intelmatix.Modules.Sidebar.Primitives.SidebarData;

namespace Intelmatix.Modules.Sidebar
{
    public class DecideOption : MonoBehaviour
    {
        // [SerializeField] private ToggleGroup toggleGroup;
        [Header("Reference")]
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Toggle toggleOption;

        [Space]
        [SerializeField] private CanvasGroup toggleCanvasGroup;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasGroup;


        private void Awake()
        {
            var alpha = 0.25f;
            var duration = 0.5f;
            toggleCanvasGroup.alpha = alpha;

            toggleOption.onValueChanged.AddListener((value) =>
            {
                toggleCanvasGroup.LeanAlpha(value ? 1 : alpha, duration);
            });
        }

        public void Display(Decision decision)
        {
            this.labelText.text = decision.Label;
            this.toggleOption.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    Debug.Log("Decision: " + decision.Label);
                    // decision?.action?.Invoke();
                }
            });
            // cognitiveButton.onValueChanged.AddListener((value) =>
            // {
            //     if (value)
            //     {
            //         cognitiveMode?.Invoke();
            //     }
            // });
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


    }
}
