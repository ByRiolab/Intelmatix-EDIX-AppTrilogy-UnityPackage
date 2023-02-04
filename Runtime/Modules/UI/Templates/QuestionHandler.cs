
using UnityEngine;
using UnityEngine.UI;
using Intelmatix.Modules.UI.Components;
using TMPro;
using static Intelmatix.Data.QuestionsData;

namespace Intelmatix.Templates
{

    public class QuestionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Toggle toggle;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;

        [Header("Components")]
        [SerializeField] private TextVerticalSizeAdjuster textVerticalSizeAdjuster;


        private void OnEnable()
        {
            if (canvasToAnimate != null)
            {
                canvasToAnimate.alpha = 0;
                canvasToAnimate.blocksRaycasts = false;
            }
        }

        public void Display(Question question, ToggleGroup toggleGroup)
        {
            if (question.IsCognitive)
            {
                this.gameObject.SetActive(false);
            }
            this.name = "<li-question> [" + question.Title + "]";
            titleText.text = question.Title;
            textVerticalSizeAdjuster.UpdateSize();
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                    question.OpenMap();
            });

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
                    // LeanTween.moveLocal(this.canvasToAnimate.gameObject, Vector3.up * 75f,duration).setEase(LeanTweenType.easeOutBack);
                    UIManager.SelectQuestion(question);
                }
                else
                {
                    LeanTween.value(gameObject, (float value) =>
                    {
                        titleText.alpha = value;
                    }, titleText.alpha, 0.25f, duration);

                    if (!toggleGroup.AnyTogglesOn())
                    {
                        UIManager.SelectQuestion(null);
                    }
                    // LeanTween.moveLocal(this.canvasToAnimate.gameObject, Vector3.zero,duration).setEase(LeanTweenType.easeOutBack);
                }
            });
        }

        public void Show(float distance = 100, float duration = 1f)
        {
            AnimationManager.AnimateIn(canvasToAnimate, duration: duration, distance: distance);
        }

        public void Hide(float distance = 100, float duration = 1f)
        {
            AnimationManager.AnimateOut(canvasToAnimate, duration: duration, distance: distance);
        }
    }
}
