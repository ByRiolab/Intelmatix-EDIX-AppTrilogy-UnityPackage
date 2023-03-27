using Intelmatix.Templates;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Intelmatix.Data.SidebarData;

namespace Intelmatix.Modules.Sidebar
{
    public class DecideOption : BaseChart<Decision>
    {
        // [SerializeField] private ToggleGroup toggleGroup;
        [Header("Reference")]
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Toggle toggleOption;

        [Space]
        [SerializeField] private CanvasGroup toggleCanvasGroup;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasGroup;


        private void Awake()
        {
            var alpha = 0.25f;
            var duration = 0.5f;
            toggleCanvasGroup.alpha = alpha;

            toggleOption.onValueChanged.AddListener((value) =>
            {
                toggleCanvasGroup.LeanAlpha(value ? 1 : alpha, duration);
            });
            
        }

        private bool isAdded = false;
        public override void Display(Decision decision)
        {
            this.labelText.text = decision.Label;
            if (!SidebarManager.Instance.CanResoreKPI)
            {
                if (PlayerPrefs.HasKey(decision.Label))
                {
                    if (PlayerPrefs.GetInt(decision.Label) == 0)
                    {
                        this.toggleOption.isOn = false;
                    }
                    else if (PlayerPrefs.GetInt(decision.Label) == 1)
                    {
                        this.toggleOption.isOn = true;
                        isAdded = true;
                    }
                }
            }
            this.toggleOption.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    
                    isAdded = true;
                    SidebarManager.AddKPIDecision(decision.KPI);
                    Debug.Log("Decision: " + decision.Label);
                    // decision?.action?.Invoke();
                    PlayerPrefs.SetInt(decision.Label, 1);
                }
                else
                {
                    if (isAdded)
                    {
                        SidebarManager.RemoveKPIDecision(decision.KPI);
                        PlayerPrefs.SetInt(decision.Label, 0);
                        Debug.Log("Decision removed: " + decision.Label);
                    }
                }
            });
            // cognitiveButton.onValueChanged.AddListener((value) =>
            // {
            //     if (value)
            //     {
            //         cognitiveMode?.Invoke();
            //     }
            // });
        }

        private void OnEnable()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                AnimationManager.AnimateIn(this.canvasGroup, direction: AnimationManager.Direction.Right);
            }
        }

        private void OnDisable()
        {
            if (canvasGroup != null)
            {
                AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Right);
            }
        }

        public LTDescr Hide()
        {
            return AnimationManager.AnimateOut(this.canvasGroup, direction: AnimationManager.Direction.Right);
        }


    }
}
