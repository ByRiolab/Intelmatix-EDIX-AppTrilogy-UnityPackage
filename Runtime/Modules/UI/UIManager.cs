using UnityEngine;
using UnityEngine.UI;

namespace Intelmatix.Modules.UI
{
    using Intelmatix.Base;
    using Intelmatix.Modules.UI.Primitives;
    using Intelmatix.Modules.UI.Templates;

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
