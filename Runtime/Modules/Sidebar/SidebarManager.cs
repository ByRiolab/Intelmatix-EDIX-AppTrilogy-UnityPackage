using System.Collections.Generic;
using UnityEngine;
using AwesomeCharts;
using UnityEngine.UI;
using Intelmatix.Modules.Sidebar.Primitives;
using Intelmatix.Modules.Sidebar.Graphics;
using Intelmatix.Exoa.Responsive;

namespace Intelmatix.Modules.Sidebar
{
    public class SidebarManager : Singleton<SidebarManager>
    {
        [Header("Data References")]
        [SerializeField] private SidebarReference sidebarReference;
        [Space(20)]

        [Header("References")]
        [SerializeField] private Transform parentOfGraphics;
        [SerializeField] private Transform parentOfGraphicsTemporal;
        [SerializeField] private ResponsiveContainer responsiveContainer;
        [SerializeField] private BakgroundAnim backgroundAnimation;
        [Space]

        [Header("Graphics")]
        [SerializeField] private StackedLineChart lineChartPrefab;
        [SerializeField] private StackedBarchart barchartPrefab;
        [SerializeField] private TableChart tableChartPrefab;

        [Header("Button Close sidebar")]
        [SerializeField] private CanvasGroup buttonToAnimate;
        [SerializeField] private Button closeButton;

        private List<StackedLineChart> instanceLineCharts = new List<StackedLineChart>();
        private List<StackedBarchart> instanceBarCharts = new List<StackedBarchart>();
        private List<TableChart> instanceTableCharts = new List<TableChart>();

        protected override void Awake()
        {
            if (sidebarReference == null)
            {
                Debug.LogWarning("Sidebar reference is null");
                DestroyImmediate(this.gameObject);
            }
            else base.Awake();
        }

        void Start()
        {
            closeButton.onClick.AddListener(CloseSidebar);

            instanceLineCharts = new();
            instanceBarCharts = new();
            instanceTableCharts = new();
        }
        void OnEnable()
        {
            parentOfGraphics.DestroyChildren();
            // foreach (var child in parentOfGraphics.GetComponentsInChildren<RectTransform>())
            //     DestroyImmediate(child.gameObject);

            sidebarReference.OnDataChanged += SetupSidebar;
            buttonToAnimate.alpha = 0;
            buttonToAnimate.blocksRaycasts = false;
        }
        void OnDisable()
        {
            sidebarReference.OnDataChanged -= SetupSidebar;
        }

        public static void Close()
        {
            Instance.CloseSidebar();
        }
        public static void ShowCloseButton()
        {
            AnimationManager.AnimateIn(Instance.buttonToAnimate, direction: AnimationManager.Direction.Up);
        }
        private void CloseSidebar()
        {
            backgroundAnimation.HideRect(AnimationSettings.Sidebar.BackgroundCloseDuration);
            AnimationManager.AnimateOut(Instance.buttonToAnimate, direction: AnimationManager.Direction.Up);
            DestroyGraphics();
        }


        private void SetupSidebar(SidebarData sidebar)
        {
            // CloseSidebar();

            backgroundAnimation.ShowRect(AnimationSettings.Sidebar.BackgroundAppearDuration);
            DestroyGraphics();

            var delay = AnimationSettings.Sidebar.ContentAppearDelay;
            var delay_between = AnimationSettings.Sidebar.DelayBetweenCharts;

            LeanTween.cancel(this.gameObject);

            sidebar.LineCharts.ForEach(lineChart =>
            {
                LeanTween.delayedCall(this.gameObject, delay, () =>
                {
                    var instance = Instantiate(lineChartPrefab, parentOfGraphics);
                    instance.Display(lineChart);
                    instanceLineCharts.Add(instance);
                    responsiveContainer.Resize(true);
                });
                delay += delay_between;
            });
            sidebar.BarCharts.ForEach(BarChart =>
            {
                LeanTween.delayedCall(this.gameObject, delay, () =>
                {
                    StackedBarchart instance = Instantiate(barchartPrefab, parentOfGraphics);
                    instance.Display(BarChart);
                    instanceBarCharts.Add(instance);
                    responsiveContainer.Resize(true);
                });
                delay += delay_between;
            });

            sidebar.TableCharts.ForEach(tableChart =>
            {
                LeanTween.delayedCall(this.gameObject, delay, () =>
                {
                    TableChart instance = Instantiate(tableChartPrefab, parentOfGraphics);
                    instance.Display(tableChart);
                    instanceTableCharts.Add(instance);
                    responsiveContainer.Resize(true);
                });
                delay += delay_between;
            });

        }



        private void DestroyGraphics()
        {
            instanceLineCharts?.ForEach(lineChart =>
            {
                lineChart.enabled = false;
                var worldPosition = lineChart.transform.position;
                lineChart.transform.SetParent(parentOfGraphicsTemporal, true);
                lineChart.transform.position = worldPosition;
                Destroy(lineChart.gameObject, AnimationSettings.Sidebar.ContentCloseDuration);
            });
            instanceBarCharts?.ForEach(BarChart =>
            {
                BarChart.enabled = false;
                var worldPosition = BarChart.transform.position;
                BarChart.transform.SetParent(parentOfGraphicsTemporal, true);
                BarChart.transform.position = worldPosition;
                Destroy(BarChart.gameObject, AnimationSettings.Sidebar.ContentCloseDuration);
            });
            instanceTableCharts?.ForEach(tableChart =>
            {
                tableChart.enabled = false;
                var worldPosition = tableChart.transform.position;
                tableChart.transform.SetParent(parentOfGraphicsTemporal, true);
                tableChart.transform.position = worldPosition;
                Destroy(tableChart.gameObject, AnimationSettings.Sidebar.ContentCloseDuration);
            });

            instanceLineCharts = new();
            instanceBarCharts = new();
            instanceTableCharts = new();
        }

    }
}


// [SerializeField] private RectTransform transformBackground;

// private void Start()
// {
//     transformBackground.LeanMoveX(1884f, 0);
// }

// private void OnEnable()
// {
//     //HideRect();
// }

// public void HideRect(float duration = 1f)
// {
//     // Ocultar el rectángulo moviéndolo a la posición final (posición x = 1884)
//     LeanTween.cancel(transformBackground);
//     LeanTween.moveX(transformBackground, 1884f, duration).setEaseInOutSine();
// }

// public void ShowRect(float duration = 1f)
// {
//     // Mostrar el rectángulo moviéndolo a la posición inicial (posición x = 0)
//     LeanTween.cancel(transformBackground);
//     LeanTween.moveX(transformBackground, 0f, duration).setEaseInOutSine();
// }