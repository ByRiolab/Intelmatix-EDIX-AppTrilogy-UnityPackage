using UnityEngine;
using Intelmatix.Modules.Questions.Primitives;
using Intelmatix.Modules.Questions.Components;
using UnityEngine.UI;

namespace Intelmatix.Modules.Questions
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Data References")]
        public QuestionsReference questionsReference;
        [Space(20)]

        [Header("Components")]
        [SerializeField] private TabHandler tabHandlerPrefab;
        [SerializeField] private QuestionsPanel questionsPanel;
        [SerializeField] private ToggleGroup toggleGroup;

        [Header("Transform References")]
        [SerializeField] private RectTransform parentOfTabs;
        [SerializeField] private RectTransform parentOfQuestions;

        private QuestionsPanel currentPanel;

        protected override void Awake()
        {
            if (questionsReference == null)
            {
                Debug.LogWarning("Questions reference is null");
                DestroyImmediate(this.gameObject);
                return;
            }
            else base.Awake();
        }

        void OnEnable()
        {
            questionsReference.OnDataChanged += SetupQuestions;
            parentOfQuestions.DestroyChildren();
        }
        void OnDisable()
        {
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

                    instance.SetOnValueChanged((value) =>
                    {
                        currentPanel?.Hide();
                        currentPanel = null;
                        if (value)
                        {
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
