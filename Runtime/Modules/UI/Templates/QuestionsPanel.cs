using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using static Intelmatix.Data.QuestionsData;

namespace Intelmatix.Templates
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestionsPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI globalQuestionText;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private RectTransform parentOfQuestions;

        [Header("Components")]
        [SerializeField] private QuestionHandler questionHandlerPrefab;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;

        private readonly List<QuestionHandler> questionHandlers = new();
        private CanvasGroup myCanvasGroup;
        private void Awake()
        {
            myCanvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            if (canvasToAnimate != null)
            {
                canvasToAnimate.alpha = 0;
                canvasToAnimate.blocksRaycasts = false;
            }
        }


        public void Display(Tab tab)
        {
            this.name = "<questions-panel> [" + tab.Tittle + "]";

            globalQuestionText.text = tab.GlobalQuestion;

            // Instantiate question handlers
            parentOfQuestions.DestroyChildren();

            for (int i = 0; i < 5; i++)
            {
                tab.Questions.ForEach(question =>
                   {
                       var instance = Instantiate(questionHandlerPrefab, parentOfQuestions);
                       instance.Display(question, toggleGroup);
                       questionHandlers.Add(instance);
                   });
            }
            Show();
        }

        private void Show()
        {
            LeanTween.cancel(gameObject);
            myCanvasGroup.alpha = 0;
            myCanvasGroup.LeanAlpha(1, .1f).setOnComplete(() =>
            {
                var duration = 1.1f;
                // Animation of cascade
                for (int i = 0; i < questionHandlers.Count; i++)
                {
                    questionHandlers[i].Show
                    (
                        duration: duration,
                        distance: 100f * (i + 2)
                    );
                }
                AnimationManager.AnimateIn(canvasToAnimate, duration: duration, distance: 100f);
            });
        }

        public void Hide()
        {
            LeanTween.cancel(gameObject);
            var duration = .8f;
            // Inverse animation of cascade
            var length = questionHandlers.Count;
            for (int i = 0; i < length; i++)
            {
                questionHandlers[i].Hide
                (
                    duration: duration,
                    distance: 100f * (length + 1 - i)
                );
            }
            AnimationManager.AnimateOut(canvasToAnimate, duration: duration, distance: (length + 2) * 100f);

            myCanvasGroup.LeanAlpha(0, .1f).setDelay(1.25f).setOnComplete(() => Destroy(gameObject));
        }
    }
}