using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Intelmatix.Modules.Sidebar.Components;
using static Intelmatix.Data.SidebarData;
using Intelmatix.Settings;

namespace Intelmatix.Templates
{
    public class StadisticsTableChart : BaseChart<Stadistics>
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Transform parentOfOptions;

        [Space]
        [Header("Components")]
        [SerializeField] private RowStadisticsHandler rowHandlerPrefab;
        [SerializeField] private ChartResizer chartResizeManager;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        public override void Display(Stadistics tableChart)
        {
            this.name = "<table-product-chart> [" + tableChart.Title + "]";
            titleText.text = tableChart.Title;

            foreach (Transform child in parentOfOptions)
            {
                child.transform.SetParent(null);
            }
            parentOfOptions.DestroyChildren();
            rowHandlerPrefab.gameObject.SetActive(false);

            // Debug.Log("verticalLayoutGroup.preferredHeight: " + verticalLayoutGroup.preferredHeight);
            foreach (var row in tableChart.Row)
            {
                var instance = Instantiate(rowHandlerPrefab, parentOfOptions);
                instance.Display(row);
                instance.gameObject.SetActive(true);
            }
            verticalLayoutGroup.SetLayoutVertical();
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentOfOptions.GetComponent<RectTransform>());
            // ContentSizeFitter fitter = verticalLayoutGroup.GetComponent<ContentSizeFitter>();
            // fitter.
            chartResizeManager.SetMaximizeSize(verticalLayoutGroup.preferredHeight);
        }

        private void OnEnable()
        {
            AnimationManager.AnimateIn(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: SidebarAnimationSettings.ContentAppearDuration);
        }

        private void OnDisable()
        {
            AnimationManager.AnimateOut(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: SidebarAnimationSettings.ContentCloseDuration);
        }
    }
}
