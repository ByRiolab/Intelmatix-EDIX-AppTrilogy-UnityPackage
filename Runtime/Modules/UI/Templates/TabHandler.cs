using Intelmatix.Modules.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Intelmatix.Modules.UI.Primitives.QuestionsData;

namespace Intelmatix.Templates
{

    public class TabHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private CanvasGroup canvasToAnimate;
        [SerializeField] private Toggle toggle;


        [Header("Components")]
        [SerializeField] private TextSizeAdjuster textSizeAdjuster;


        private void OnEnable()
        {
            canvasToAnimate.alpha = 0f;

            AnimationManager.AnimateIn
            (
                canvasGroup: this.canvasToAnimate,
                direction: AnimationManager.Direction.Up,
                easeType: LeanTweenType.easeInOutSine,
                duration: 1f
            );
        }

        private void OnDisable()
        {
            AnimationManager.AnimateOut(this.canvasToAnimate);
        }

        public void Display(Tab tab, ToggleGroup toggleGroup)
        {
            this.name = "<tab> [" + tab.Tittle + "]";
            titleText.text = tab.Tittle + ".";
            textSizeAdjuster.UpdateSize();

            toggle.group = toggleGroup;

            // LeanTween.alphaCanvas(canvasToAnimate, 0.25f, 0f);
            titleText.alpha = .25f;
            var duration = 0.5f;
            toggle.onValueChanged.AddListener((value) =>
            {
                LeanTween.cancel(this.canvasToAnimate.gameObject);
                if (value)
                {
                    LeanTween.value(gameObject, (float value) =>
                    {
                        titleText.alpha = value;
                    }, titleText.alpha, 1f, duration);
                    LeanTween.moveLocal(this.canvasToAnimate.gameObject, Vector3.up * 75f, duration).setEase(LeanTweenType.easeOutBack);
                }
                else
                {
                    LeanTween.value(gameObject, (float value) =>
                    {
                        titleText.alpha = value;
                    }, titleText.alpha, 0.25f, duration);
                    LeanTween.moveLocal(this.canvasToAnimate.gameObject, Vector3.zero, duration).setEase(LeanTweenType.easeOutBack);
                }
            });
        }

        public void SetOnValueChanged(UnityAction<bool> onValueChanged)
        {
            toggle.onValueChanged.AddListener(onValueChanged);
        }
    }

}
