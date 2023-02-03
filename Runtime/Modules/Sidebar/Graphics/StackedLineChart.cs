using System.Collections.Generic;
using Intelmatix.Modules.Sidebar.Primitives;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using TMPro;
using System.Linq;

namespace Intelmatix.Modules.Sidebar.Graphics
{
    public class StackedLineChart : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private Transform parentOfOptions;

        [Space]
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Toggle togglePrefab;

        [Space]
        [SerializeField] private LineChart lineChartTemplate;


        [Header("Animation")]
        [SerializeField] private CanvasGroup canvasToAnimate;


        private void Start()
        {
            AnimationManager.AnimateIn(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: SidebarAnimationSettings.ContentAppearDuration);
            togglePrefab.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            canvasToAnimate.alpha = 0;
        }
        private void OnDisable()
        {
            // if (this.gameObject != null && this.canvasToAnimate == null)
            //     return;
            AnimationManager.AnimateOut(this.canvasToAnimate, direction: AnimationManager.Direction.Right, duration: SidebarAnimationSettings.ContentCloseDuration);
        }

        internal void Display(SidebarData.ChartGroup lineChart)
        {
            this.name = "<line-chart> [" + lineChart.Title + "]";
            titleText.text = lineChart.Title;
            subtitleText.text = lineChart.Subtitle;


            if (lineChart.Charts.Count > 0)
            {
                FillTemplate(lineChart.Charts[0]);

                // create toggles
                parentOfOptions.DestroyChildren();
                var toggles = new List<Toggle>();
                foreach (var chart in lineChart.Charts)
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
            }
            else
            {
                // Debug.LogWarning("No charts found for line chart group: " + lineChart.Title);
            }
        }

        private void FillTemplate(SidebarData.Chart chart)
        {
            chart.ApplyAxisConfiguration(lineChartTemplate);

            // bind data
            lineChartTemplate.GetChartData().Clear();

            var dataSets = chart.GetLineDataSets();
            foreach (var dataSet in dataSets)
            {
                lineChartTemplate.GetChartData().DataSets.Add(dataSet);
            }
            lineChartTemplate.SetDirty();

        }


        // private void FillLineChart(int i, int item)
        // {
        //     LineDataSet dataSet = new();
        //     lineChart[i].GetChartData().Clear();
        //     lineChart[i].GridConfig.HorizontalLinesCount = 0; // unnecessary
        //     lineChart[i].GridConfig.VerticalLinesCount = 0; // unnecessary

        //     lineChart[i].AxisConfig.HorizontalAxisConfig.Bounds.Max = sidebarReference.Data.Store.LineCharts[i].Charts[item].AxisConfig.HorizontalAxisconfig.Max;
        //     lineChart[i].AxisConfig.HorizontalAxisConfig.Bounds.Min = sidebarReference.Data.Store.LineCharts[i].Charts[item].AxisConfig.HorizontalAxisconfig.Min;
        //     lineChart[i].AxisConfig.VerticalAxisConfig.Bounds.MaxAutoValue = true;
        //     lineChart[i].AxisConfig.VerticalAxisConfig.Bounds.MinAutoValue = true;
        //     lineChart[i].Config.ValueIndicatorSize = 0;
        //     lineChart[i].AxisConfig.HorizontalAxisConfig.LabelsCount = sidebarReference.Data.Store.LineCharts[i].Charts[item].AxisConfig.HorizontalAxisconfig.CustomValues.Length;
        //     lineChart[i].AxisConfig.VerticalAxisConfig.LabelsCount = (int)sidebarReference.Data.Store.LineCharts[i].Charts[item].AxisConfig.HorizontalAxisconfig.LavelsCount;

        //     for (int a = 0; a < sidebarReference.Data.Store.LineCharts[i].Charts[item].AxisConfig.HorizontalAxisconfig.CustomValues.Length; a++)
        //     {
        //         lineChart[i].AxisConfig.HorizontalAxisConfig.ValueFormatterConfig.CustomValues.Add(sidebarReference.Data.Store.LineCharts[i].Charts[item].AxisConfig.HorizontalAxisconfig.CustomValues[a]);
        //     }
        //     for (int a = 0; a < sidebarReference.Data.Store.LineCharts[i].Charts[item].Data.DataSets[0].Entries.Length; a++)
        //     {
        //         dataSet.AddEntry(new LineEntry(sidebarReference.Data.Store.LineCharts[i].Charts[item].Data.DataSets[0].Entries[a].Position, sidebarReference.Data.Store.LineCharts[i].Charts[item].Data.DataSets[0].Entries[a].Value));
        //     }
        //     dataSet.LineColor = Color.red;
        //     lineChart[i].GetChartData().DataSets.Add(dataSet);
        //     dataSet.UseBezier = true;
        //     lineChart[i].SetDirty();
        // }

    }
}
