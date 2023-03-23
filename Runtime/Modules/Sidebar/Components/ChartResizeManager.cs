using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using Intelmatix.Exoa.Responsive;
using System.Linq;

namespace Intelmatix.Modules.Sidebar.Components
{
    public class ChartResizeManager : MonoBehaviour
    {
        [SerializeField] private Button minimizeButton;
        [SerializeField] private Button maximizeButton;
        [SerializeField] private ChartResizer chartResizer;
        [SerializeField] private ResponsiveContainer responsiveContainer;
        [SerializeField] private LineChart lineChart;
        [SerializeField] private BarChart barChart;

        private void Start()
        {
            // Switching between minimize and maximize
            minimizeButton.onClick.AddListener(chartResizer.MinimizeChart);
            maximizeButton.onClick.AddListener(chartResizer.MaximizeChart);

            minimizeButton.onClick.AddListener(() =>
            {
                minimizeButton.gameObject.SetActive(false);
                maximizeButton.gameObject.SetActive(true);

                // lineChart?.HorizontalDisplay(false);

            });
            maximizeButton.onClick.AddListener(() =>
            {
                minimizeButton.gameObject.SetActive(true);
                maximizeButton.gameObject.SetActive(false);

                // lineChart?.HorizontalDisplay(true);
            });

            chartResizer.OnChartResizeUpdate += ManualResize;

            // chartResizer.MinimizeChart();
        }

        private void OnDisable()
        {
            chartResizer.OnChartResizeUpdate -= ManualResize;
        }

        private void ManualResize(float size)
        {
            responsiveContainer?.Resize(false);

            lineChart?.SetLabelOpacity(size);
            lineChart?.SetDirty();

            if (barChart)
            {
                var width = barChart.GetComponent<RectTransform>().sizeDelta.x;
                var length = barChart.GetChartData().DataSets.First().Entries.Count + 1;
                barChart.Config.BarSpacing = (int)(width / length) / 3;
                barChart.SetLabelOpacity(size);
                barChart.SetDirty();
                // Debug.Log("width: " + width + " length: " + length + " barChart.Config.BarSpacing: " + barChart.Config.BarSpacing);

            }

        }
    }
}