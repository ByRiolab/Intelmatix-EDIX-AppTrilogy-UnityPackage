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
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private RectTransform parentOfQuestions;
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        [Header("Components")]
        [SerializeField] private QuestionHandler questionHandlerPrefab;

        private readonly List<QuestionHandler> questionHandlers = new();
        private CanvasGroup myCanvasGroup;
        private void Awake()
        {
            myCanvasGroup = GetComponent<CanvasGroup>();
            simpleScrollSnap = simpleScrollSnap != null ? simpleScrollSnap : GetComponent<SimpleScrollSnap>();
        }

        public void Display(Tab tab)
        {
            this.name = "<questions-panel> [" + tab.Tittle + "]";

            for (int i = simpleScrollSnap.Content.childCount - 1; i >= 0; i--)
            {
                try
                {
                    simpleScrollSnap.RemoveFromFront();
                }
                catch { }
            }

            // Instantiate question handlers
            tab.Questions.ForEach(question =>
               {
                   simpleScrollSnap.AddToBack(questionHandlerPrefab.gameObject);

                   QuestionHandler instance = simpleScrollSnap.Content.GetChild(simpleScrollSnap.Content.childCount - 1).GetComponent<QuestionHandler>();

                   instance.Display(question, toggleGroup);
                   questionHandlers.Add(instance);
               });

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
            });
        }

        public void Hide()
        {
            LeanTween.cancel(gameObject);
            myCanvasGroup.LeanAlpha(0, .5f).setOnComplete(() =>
            {
                for (int i = simpleScrollSnap.Content.childCount - 1; i >= 0; i--)
                {
                    try
                    {
                        simpleScrollSnap.RemoveFromFront();
                    }
                    catch { }
                }
                Destroy(gameObject);
            });
        }
    }
}