using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using static Intelmatix.Data.QuestionsData;
using DanielLochner.Assets.SimpleScrollSnap;

namespace Intelmatix.Templates
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestionsPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI globalQuestionText;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private RectTransform parentOfQuestions;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

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
                       simpleScrollSnap.AddToFront(questionHandlerPrefab.gameObject);

                       var instance = simpleScrollSnap.Content.GetChild(simpleScrollSnap.Content.childCount - 1).GetComponent<QuestionHandler>();

                       instance.Display(question, toggleGroup);
                       questionHandlers.Add(instance);
                   });
            }
            if (TryGetComponent(out InfiniteScroll infiniteScroll))
            {
                infiniteScroll.originalItemsCount = tab.Questions.Count;
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

            myCanvasGroup.LeanAlpha(0, .1f).setDelay(1.25f);
            Destroy(gameObject, 1.3f);
        }
    }
}