using UnityEngine;


namespace Intelmatix.Modules.Sidebar.Components
{
    /// <summary>
    /// This class is used to resize the chart in the sidebar.
    /// @author: Anthony Shinomiya M.
    /// @date: 2022-01-22
    /// </summary>
    public class ChartResizer : MonoBehaviour
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        [SerializeField] private RectTransform chart;
        [SerializeField] private float minimizeSize = 300;
        [SerializeField] private float maximizeSize = 600;
        [SerializeField] private float animationTime = 0.5f;
        [SerializeField] private LeanTweenType animationType = LeanTweenType.easeOutCubic;
        [SerializeField] private Direction direction;

        private float marginBottom = 60;

        public delegate void ChartResizeEvent(float size);
        public event ChartResizeEvent OnChartResizeStart;
        public event ChartResizeEvent OnChartResizeUpdate;
        public event ChartResizeEvent OnChartResizeEnd;

        public void SetMaximizeSize(float newValue)
        {
            if (newValue > minimizeSize && newValue < maximizeSize)
            {
                this.maximizeSize = newValue + marginBottom;
            }
            // Debug.Log("verticalLayoutGroup.preferredHeight: " + this.maximizeSize);
        }

        public void ResizeChart(float targetSize)
        {
            if (chart.sizeDelta.y == targetSize)
            {
                return;
            }
            OnChartResizeStart?.Invoke(chart.sizeDelta.y);

            if (direction == Direction.Vertical)
            {
                LeanTween.value(chart.gameObject, (float value) =>
                {
                    chart.sizeDelta = new Vector2(chart.sizeDelta.x, value);
                    OnChartResizeUpdate?.Invoke(value);
                }, chart.sizeDelta.y, targetSize, animationTime).setEase(animationType).setOnComplete(() =>
                {
                    OnChartResizeEnd?.Invoke(targetSize);
                });
            }
            else
            {
                LeanTween.value(chart.gameObject, (float value) =>
                {
                    chart.sizeDelta = new Vector2(value, chart.sizeDelta.y);
                    OnChartResizeUpdate?.Invoke(value);
                }, chart.sizeDelta.x, targetSize, animationTime).setEase(animationType).setOnComplete(() =>
                {
                    OnChartResizeEnd?.Invoke(targetSize);
                });
            }


        }

        public void MaximizeChart()
        {
            ResizeChart(maximizeSize);
        }

        public void MinimizeChart()
        {
            ResizeChart(minimizeSize);
        }
    }
}