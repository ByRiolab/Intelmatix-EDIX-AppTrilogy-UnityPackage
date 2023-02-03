using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Intelmatix.Modules.Sidebar
{
    public class DecideOptions : MonoBehaviour
    {
        [Header("Toggle")]
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Toggle humanButton;
        [SerializeField] private Toggle cognitiveButton;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasGroup;
        // [SerializeField] private Button button;

        public void Display(UnityEngine.Events.UnityAction humanMonde, UnityEngine.Events.UnityAction cognitiveMode)
        {
            // humanButton.onClick.RemoveAllListeners();
            // humanButton.onClick.AddListener(onClick);

            humanButton.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    humanMonde?.Invoke();
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


    }
}
