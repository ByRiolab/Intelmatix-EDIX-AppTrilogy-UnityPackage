
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Intelmatix.Data.QuestionsData;

namespace Intelmatix.Templates
{

    public class QuestionHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Toggle toggle;

        public void Display(Question question, ToggleGroup toggleGroup)
        {
            if (question.IsCognitive)
            {
                this.gameObject.SetActive(false);
            }

            this.name = "<li-question> [" + question.Title + "]";
            titleText.text = question.Title;
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
                if (value)
                {
                    LeanTween.value(gameObject, (float value) =>
                    {
                        titleText.alpha = value;
                    }, titleText.alpha, 1f, duration);

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
                }
            });
        }
    }
}
