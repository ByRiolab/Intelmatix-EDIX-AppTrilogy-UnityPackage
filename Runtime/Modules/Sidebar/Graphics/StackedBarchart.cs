using System.Collections.Generic;
using Intelmatix.Modules.Sidebar.Primitives;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using TMPro;
using System.Linq;
using Intelmatix;
namespace Intelmatix.Templates
{
    public class StackedBarchart : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private Transform parentOfOptions;

        [Space]
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Toggle togglePrefab;

        [Space]
        [SerializeField] private BarChart BarChartTemplate;


        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;

        private void OnEnable()
        {
            AnimationManager.AnimateIn(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: SidebarAnimationSettings.ContentAppearDuration);
            togglePrefab.gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            if (this.canvasToAnimate == null)
                return;
            AnimationManager.AnimateOut(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: SidebarAnimationSettings.ContentCloseDuration);
        }

        internal void Display(SidebarData.ChartGroup barChart)
        {
            this.name = "<bar-chart> [" + barChart.Title + "]";
            titleText.text = barChart.Title;
            subtitleText.text = barChart.Subtitle;


            if (barChart.Charts.Count > 0)
            {
                FillTemplate(barChart.Charts[0]);

                // create toggles
                parentOfOptions.DestroyChildren();
                var toggles = new List<Toggle>();
                foreach (var chart in barChart.Charts)
                {
                    var toggle = Instantiate(togglePrefab, parentOfOptions);
                    toggle.group = toggleGroup;
                    toggle.onValueChanged.AddListener((value) =>
                    {
                        if (value)
                            FillTemplate(chart);
                    });
                    toggle.GetComponentInChildren<TextMeshProUGUI>().text = chart.Item;
                    toggle.gameObject.SetActive(true);
                    toggles.Add(toggle);
                }
                toggles.First().isOn = true;
                if (toggles.Count == 1)
                {
                    toggles.First().gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("No charts found for line chart group: " + barChart.Title);
            }
        }
        // Update is called once per frame

        private void FillTemplate(SidebarData.Chart chart)
        {
            chart.ApplyBarAxisConfiguration(BarChartTemplate);

            // bind data
            BarChartTemplate.GetChartData().Clear();

            var dataSets = chart.GetBarDataSets();
            foreach (var dataSet in dataSets)
            {
                BarChartTemplate.GetChartData().DataSets.Add(dataSet);
            }
            BarChartTemplate.SetDirty();

        }

    }
}
