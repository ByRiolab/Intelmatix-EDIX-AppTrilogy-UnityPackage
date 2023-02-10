using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using TMPro;
using System.Linq;
using Intelmatix;
using Intelmatix.Data;
using Intelmatix.Settings;
using Intelmatix.Modules.Sidebar.Components;

namespace Intelmatix.Templates
{
    public class StackedBarchart : BaseChart<SidebarData.ChartGroup>
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
        [SerializeField] private ChartResizer chartResizer;
        [SerializeField] private LegendView legendView;


        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;


        [Header("Hieght")]
        [SerializeField] private float minHeight = 280;
        [SerializeField] private float maxHeight = 330;
        [SerializeField] private GameObject contentOfLegend;
        [SerializeField] private RectTransform contentOfRectResponsive;
        [SerializeField] private RectTransform contentOfParent;


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

        public override void Display(SidebarData.ChartGroup barChart)
        {
            this.name = "<bar-chart> [" + barChart.Title + "]";
            titleText.text = barChart.Title;
            subtitleText.text = barChart.Subtitle;


            if (barChart.Charts.Count > 0)
            {
                FillTemplate(barChart.Charts[0]);
                try
                {
                    if (barChart.Charts.First().Data.DataSets.Length > 1)
                    {
                        contentOfLegend.SetActive(true);
                        this.contentOfRectResponsive.sizeDelta = new Vector2(this.contentOfRectResponsive.sizeDelta.x, maxHeight);
                        this.contentOfParent.sizeDelta = new Vector2(this.contentOfParent.sizeDelta.x, maxHeight);
                     
                    }
                    else
                    {
                        contentOfLegend.SetActive(false);
                        this.contentOfRectResponsive.sizeDelta = new Vector2(this.contentOfRectResponsive.sizeDelta.x, minHeight);
                        this.contentOfParent.sizeDelta = new Vector2(this.contentOfParent.sizeDelta.x, minHeight);
                    }
                }
                catch
                {
                    this.contentOfRectResponsive.sizeDelta = new Vector2(this.contentOfRectResponsive.sizeDelta.x, minHeight);
                    this.contentOfParent.sizeDelta = new Vector2(this.contentOfParent.sizeDelta.x, minHeight);
                    contentOfLegend.SetActive(false);
                }

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

            // bind data
            BarChartTemplate.GetChartData().Clear();

            var dataSets = chart.GetBarDataSets();
            foreach (var dataSet in dataSets)
            {
                BarChartTemplate.GetChartData().DataSets.Add(dataSet);
            }
            BarChartTemplate.legendView = legendView;
            BarChartTemplate.SetDirty();
            chart.ApplyBarAxisConfiguration(BarChartTemplate, chartResizer.CurrentPercentage);
            

        }

    }
}
