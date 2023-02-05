using UnityEngine;
using UnityEngine.UI;

namespace Intelmatix
{
    using Intelmatix.Base;
    using Intelmatix.Data;
    using Intelmatix.Templates;
    using static Intelmatix.Data.QuestionsData;

    public class UIManager : Singleton<UIManager, QuestionsReference>
    {
        // [Header("Data References")]
        // public QuestionsReference questionsReference;
        // [Space(20)]

        [Header("Components")]
        [SerializeField] private TabHandler tabHandlerPrefab;
        [SerializeField] private QuestionsPanel questionsPanel;
        [SerializeField] private ToggleGroup toggleGroup;

        [Header("Transform References")]
        [SerializeField] private RectTransform parentOfTabs;
        [SerializeField] private RectTransform parentOfQuestions;

        private QuestionsPanel currentPanel;


        private QuestionsReference questionsReference => dataReference;

        #region Callbacks
        // public delegate void OnValueChanged(bool value, ToggleGroup toggleGroup);
        public delegate void OnQuestionSelectedEvent(Question question);
        public static event OnQuestionSelectedEvent OnQuestionSelected;

        public delegate void OnTabSelectedEvent(Tab tab);
        public static event OnTabSelectedEvent OnTabSelected;
        #endregion

        public static void SelectQuestion(Question question)
        {
            OnQuestionSelected?.Invoke(question);
        }
        void OnEnable()
        {
            if (Object.ReferenceEquals(questionsReference, null)) return;
            questionsReference.OnDataChanged += SetupQuestions;
            parentOfQuestions.DestroyChildren();
        }
        void OnDisable()
        {
            if (Object.ReferenceEquals(questionsReference, null)) return;
            questionsReference.OnDataChanged -= SetupQuestions;
        }

        void SetupQuestions(QuestionsData questionsData)
        {
            //Clear tabs
            parentOfTabs.DestroyChildren();

            //Instantiate tabs on the bottom of the screen
            var delayBetweenTabs = .5f;
            questionsData.Tabs.ForEach(tab =>
            {
                LeanTween.delayedCall(delayBetweenTabs, () =>
                {
                    var instance = Instantiate(tabHandlerPrefab, parentOfTabs);

                    instance.Display(tab, toggleGroup);

                    instance.SetOnValueChanged((isSelected) =>
                    {
                        if (isSelected)
                        {
                            OnTabSelected?.Invoke(tab);
                        }
                        else if (!toggleGroup.AnyTogglesOn())
                        {
                            OnTabSelected?.Invoke(null);
                        }

                        currentPanel?.Hide();
                        currentPanel = null;
                        // currentPanel?.Hide();
                        // currentPanel = null;

                        // var isCognitive = tab.Questions.TrueForAll(question => question.IsCognitive == true);
                        var cognitiveQuestion = tab.Questions.Find(question => question.IsCognitive == true);
                        var isCognitive = cognitiveQuestion != null;
                        // Debug.Log("Is cognitive: " + isCognitive);

                        if (isCognitive && isSelected)
                        {
                            SidebarManager.OpenDecisionPanel(
                                humanMonde: () =>
                                {
                                    currentPanel?.Hide();
                                    currentPanel = null;
                                    currentPanel = Instantiate(questionsPanel, parentOfQuestions);
                                    currentPanel.Display(tab);
                                    OnTabSelected?.Invoke(tab);
                                },
                                cognitiveMode: () =>
                                {
                                    currentPanel?.Hide();
                                    currentPanel = null;
                                    OnQuestionSelected?.Invoke(cognitiveQuestion);
                                }
                            );

                        }
                        else if (isSelected)
                        {

                            currentPanel?.Hide();
                            currentPanel = null;
                            currentPanel = Instantiate(questionsPanel, parentOfQuestions);
                            currentPanel.Display(tab);
                        }
                        else
                        {
                            // if (!toggleGroup.AnyTogglesOn())
                            // {
                            // }
                        }
                    });
                });
                delayBetweenTabs += 0.075f;
            });
        }

    }
}
