using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using TMPro;
using System.Linq;
using Intelmatix.Data;
using Intelmatix.Settings;
using Intelmatix.Modules.Sidebar.Components;

namespace Intelmatix.Templates
{
    public class StackedLineChart : BaseChart<SidebarData.ChartGroup>
    {

        [Header("References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private Transform parentOfOptions;

        [Space]
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Toggle togglePrefab;
        [SerializeField] private TMP_Dropdown dropdown;

        [Space]
        [SerializeField] private LineChart lineChartTemplate;
        [SerializeField] private LineChart lineChartLabel;
        [SerializeField] private RectTransform lineChartContainer;
        [SerializeField] private ChartResizer chartResizer;
        [SerializeField] private ChartResizer chartResizerDropdown;


        private List<string> paths;
        private List<string> options;

        public static int contentDropdown;


        public delegate void OnDropdownSelectedEvent(string path, int Content);
        public static event OnDropdownSelectedEvent PreDropdownSelectedEvent;


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
        private bool _hasBeenExpanded = false;
        private void Update()
        {
            if (dropdown.IsExpanded && !_hasBeenExpanded)
            {
                chartResizerDropdown.MaximizeChart();
                _hasBeenExpanded = true;
            }
            else if (!dropdown.IsExpanded && _hasBeenExpanded)
            {
                chartResizerDropdown.MinimizeChart();
                _hasBeenExpanded = false;
            }

            // _hasBeenExpanded = dropdown.IsExpanded;
        }

        public override void Display(SidebarData.ChartGroup lineChart)
        {
            this.name = "<line-chart> [" + lineChart.Title + "]";
            titleText.text = lineChart.Title;
            subtitleText.text = lineChart.Subtitle;
            if (SidebarManager.filters.Count == 0)
            {
                dropdown.gameObject.SetActive(false);
            }
            else
            {
                dropdown.gameObject.SetActive(true);
                paths = new();
                options = new();
                Debug.Log(SidebarManager.filters[0].Path);
                for (int i = 0; i < SidebarManager.filters.Count; i++)
                {
                    paths.Add(SidebarManager.filters[i].Path);
                    options.Add(SidebarManager.filters[i].Filter);

                }
                dropdown.AddOptions(options);
                dropdown.value = contentDropdown;
                dropdown.onValueChanged.AddListener(SelectFromDropdown);
            }



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
                    var rect = toggle.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + 35);

                }
                toggles.First().isOn = true;
                if (toggles.Count == 1)
                {
                    toggles.First().gameObject.SetActive(false);
                }
            }
            else
            {
                // Debug.LogWarning("No charts found for line chart group: " + lineChart.Title);
            }
        }

        void SelectFromDropdown(int value)
        {
            PreDropdownSelectedEvent?.Invoke(paths[value], value);
            Debug.Log(options[value]);
            dropdown.value = value;
            contentDropdown = value;

        }

        private void FillTemplate(SidebarData.Chart chart)
        {
            chart.ApplyAxisConfiguration(lineChartTemplate, chartResizer.CurrentPercentage);
            chart.ApplyAxisConfiguration(lineChartLabel, chartResizer.CurrentPercentage);

            // bind data
            lineChartTemplate.GetChartData().Clear();
            lineChartLabel.GetChartData().Clear();

            var dataSets = chart.GetLineDataSets();
            foreach (var dataSet in dataSets)
            {
                lineChartTemplate.GetChartData().DataSets.Add(dataSet);
                lineChartLabel.GetChartData().DataSets.Add(dataSet);
            }
            if (chart.Data.DataSets.First().Entries.Count() < 40)
            {
                Debug.Log("Less than 40 data sets, data set count: " + lineChartTemplate.GetChartData().DataSets.Count);
                // fill parent container
                lineChartContainer.anchorMin = new Vector2(0, 0);
                lineChartContainer.anchorMax = new Vector2(1, 1);
                lineChartContainer.offsetMin = new Vector2(0, 0);
                lineChartContainer.offsetMax = new Vector2(0, 0);
            }
            else
            {
                // pivot on left, maximum height & set the with to the width of the chart 17000
                lineChartContainer.anchorMin = new Vector2(0, 0);
                lineChartContainer.anchorMax = new Vector2(0, 1);
                lineChartContainer.offsetMin = new Vector2(0, 0);
                lineChartContainer.offsetMax = new Vector2(17000, 0);
            }
            lineChartTemplate.SetDirty();
            lineChartLabel.SetDirty();
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
