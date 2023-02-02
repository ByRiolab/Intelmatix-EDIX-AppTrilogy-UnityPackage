using UnityEngine;
using UnityEngine.UI;
using AwesomeCharts;
using Intelmatix.Exoa.Responsive;

namespace Intelmatix.Modules.Sidebar.Components
{
    public class ChartResizeManager : MonoBehaviour
    {
        [SerializeField] private Button minimizeButton;
        [SerializeField] private Button maximizeButton;
        [SerializeField] private ChartResizer chartResizer;
        [SerializeField] private ResponsiveContainer responsiveContainer;
        [SerializeField] private LineChart lineChart;

        private void Start()
        {
            // Switching between minimize and maximize
            minimizeButton.onClick.AddListener(chartResizer.MinimizeChart);
            maximizeButton.onClick.AddListener(chartResizer.MaximizeChart);

            minimizeButton.onClick.AddListener(() =>
            {
                minimizeButton.gameObject.SetActive(false);
                maximizeButton.gameObject.SetActive(true);


                if (lineChart)
                {
                    var lenght = lineChart.AxisConfig.HorizontalAxisConfig.LabelsCount;
                    if (lenght > 7)
                    {
                        var labelConfig = lineChart.AxisConfig.HorizontalAxisConfig.LabelsConfig;

                        LeanTween.cancel(this.gameObject);
                        LeanTween.value(this.gameObject, (float val) =>
                        {
                            labelConfig.LabelColor = new Color(1, 1, 1, val);
                        }, labelConfig.LabelColor.a, 0, 0.2f).setOnComplete(() =>
                        {
                            labelConfig.LabelColor = new Color(1, 1, 1, 0);
                        }).setEase(LeanTweenType.easeOutQuad);
                    }
                }
            });
            maximizeButton.onClick.AddListener(() =>
            {
                minimizeButton.gameObject.SetActive(true);
                maximizeButton.gameObject.SetActive(false);

                if (lineChart)
                {
                    var lenght = lineChart.AxisConfig.HorizontalAxisConfig.LabelsCount;
                    if (lenght > 7)
                    {
                        var labelConfig = lineChart.AxisConfig.HorizontalAxisConfig.LabelsConfig;

                        LeanTween.cancel(this.gameObject);
                        LeanTween.value(this.gameObject, (float val) =>
                        {
                            labelConfig.LabelColor = new Color(1, 1, 1, val);
                        }, labelConfig.LabelColor.a, 1, 0.2f).setOnComplete(() =>
                        {
                            labelConfig.LabelColor = new Color(1, 1, 1, 1);
                        }).setEase(LeanTweenType.easeOutQuad);
                    }
                }
            });

            chartResizer.OnChartResizeUpdate += ManualResize;
        }

        private void OnDisable()
        {
            chartResizer.OnChartResizeUpdate -= ManualResize;
        }

        private void ManualResize()
        {
            responsiveContainer?.Resize(false);
            lineChart?.SetDirty();

        }
    }
}