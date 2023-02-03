using System.Collections;
using System.Collections.Generic;
using Intelmatix.Modules.Sidebar.Components;
using TMPro;
using UnityEngine;
using static Intelmatix.Modules.Sidebar.Primitives.SidebarData;
using UnityEngine.UI;

namespace Intelmatix.Modules.Sidebar.Graphics
{
    public class DataTableChart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Transform parentOfOptions;

        [Space]
        [Header("Components")]
        [SerializeField] private Components.RowHandler rowHandlerPrefab;
        [SerializeField] private ChartResizer chartResizeManager;

        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

        public void Display(DataTable tableChart)
        {
            this.name = "<table-data-chart> [" + tableChart.Title + "]";
            titleText.text = tableChart.Title;

            foreach (Transform child in parentOfOptions)
            {
                child.transform.SetParent(null);
            }
            parentOfOptions.DestroyChildren();
            rowHandlerPrefab.gameObject.SetActive(false);

            // Debug.Log("verticalLayoutGroup.preferredHeight: " + verticalLayoutGroup.preferredHeight);
            foreach (var row in tableChart.Rows)
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
