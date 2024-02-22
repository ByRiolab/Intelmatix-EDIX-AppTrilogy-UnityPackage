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
        [SerializeField] private Image line;

        private readonly List<QuestionHandler> questionHandlers = new();
        private CanvasGroup myCanvasGroup;
        private bool interactable = true;

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
                   if (!question.IsCognitive)
                   {
                       simpleScrollSnap.AddToBack(questionHandlerPrefab.gameObject);

                       QuestionHandler instance = simpleScrollSnap.Content.GetChild(simpleScrollSnap.Content.childCount - 1).GetComponent<QuestionHandler>();

                       instance.Display(question, toggleGroup);
                       questionHandlers.Add(instance);
                   }
               });

            Show();
        }

        private void Show()
        {
            LeanTween.cancel(gameObject);
            myCanvasGroup.alpha = 0;
            myCanvasGroup.LeanAlpha(1, .5f).setOnComplete(() =>
            {
                myCanvasGroup.blocksRaycasts = true;
            });
        }

        public void Hide()
        {
            myCanvasGroup.blocksRaycasts = false;
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
                questionHandlers.Clear();
                Destroy(gameObject);
            });
        }
        /// <summary>
        /// Changes the state of visibility and loading of this panel. Use this according the proyect needs.
        /// </summary>
        /// <param name="interactable">
        /// Is this panel interatable right now?
        /// </param>
        public void ReportInteractivity(bool interactable)
        {
            this.interactable = interactable;
            if (interactable)
            {
                LeanTween.cancel(line.gameObject);
                line.fillAmount = 1;
            }
            else
            {
                line.fillAmount = 0;
                LeanTween.cancel(line.gameObject);
                LeanTween.value(line.gameObject, 0, 1, 1.5f).setOnUpdate((float value) => line.fillAmount = value);
            }
        }

        private void LateUpdate()
        {
            if (myCanvasGroup.blocksRaycasts)
            {
                myCanvasGroup.alpha = interactable ? 1 : 0.5f;
            }
        }
    }
}