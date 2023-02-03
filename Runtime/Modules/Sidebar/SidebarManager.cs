using System.Collections.Generic;
using UnityEngine;
using AwesomeCharts;
using UnityEngine.UI;
using Intelmatix.Modules.Sidebar.Primitives;
using Intelmatix.Modules.Sidebar.Graphics;
using Intelmatix.Exoa.Responsive;
using Intelmatix.Base;
using Intelmatix.Modules.UI;
using static Intelmatix.Modules.UI.Primitives.QuestionsData;

namespace Intelmatix.Modules.Sidebar
{
    public class SidebarManager : Singleton<SidebarManager, SidebarReference>
    {
        private SidebarReference sidebarReference => dataReference;

        [Header("References")]
        [SerializeField] private Transform parentOfGraphics;
        [SerializeField] private Transform parentOfGraphicsTemporal;
        [SerializeField] private ResponsiveContainer responsiveContainer;
        [SerializeField] private BakgroundAnim backgroundAnimation;
        [Space]

        [Header("Graphics")]
        [SerializeField] private StackedLineChart lineChartPrefab;
        [SerializeField] private StackedBarchart barchartPrefab;
        [SerializeField] private ProductTableChart tableChartPrefab;
        [Space]
        [SerializeField] private DecideOptions decideOptionsPrefab;

        private List<StackedLineChart> instanceLineCharts = new List<StackedLineChart>();
        private List<StackedBarchart> instanceBarCharts = new List<StackedBarchart>();
        private List<ProductTableChart> instanceTableCharts = new List<ProductTableChart>();
        private List<DecideOptions> instanceDecideOptions = new List<DecideOptions>();


        // #region Delegates and Events
        // #endregion
        // public delegate void OnSidebarClosed();
        // public static event OnSidebarClosed OnSidebarClosedEvent;

        // public delegate void OnSidebarOpened();
        // public static event OnSidebarOpened OnSidebarOpenedEvent;


        void Start()
        {
            // closeButton.onClick.AddListener(CloseSidebar);

            instanceLineCharts = new();
            instanceBarCharts = new();
            instanceTableCharts = new();
        }
        void OnEnable()
        {
            if (Object.ReferenceEquals(sidebarReference, null)) return;
            parentOfGraphics.DestroyChildren();

            sidebarReference.OnDataChanged += SetupSidebar;
            UIManager.OnTabSelected += OnTabSelected;
            // buttonToAnimate.alpha = 0;
            // buttonToAnimate.blocksRaycasts = false;
        }
        void OnDisable()
        {
            if (Object.ReferenceEquals(sidebarReference, null)) return;
            sidebarReference.OnDataChanged -= SetupSidebar;
            UIManager.OnTabSelected -= OnTabSelected;
        }
        private void OnTabSelected(Tab tab)
        {
            CloseSidebar();
        }


        public static void Close()
        {
            Instance.CloseSidebar();
        }
        public static void ShowCloseButton()
        {
            // AnimationManager.AnimateIn(Instance.buttonToAnimate, direction: AnimationManager.Direction.Up);
        }
        private void CloseSidebar()
        {
            backgroundAnimation.HideRect(SidebarAnimationSettings.BackgroundCloseDuration);
            // AnimationManager.AnimateOut(Instance.buttonToAnimate, direction: AnimationManager.Direction.Up);
            DestroyGraphics();
        }

        public static void OpenDecisionPanel(UnityEngine.Events.UnityAction humanMonde, UnityEngine.Events.UnityAction cognitiveMode)
        {
            Instance.backgroundAnimation.ShowRect(SidebarAnimationSettings.BackgroundAppearDuration);
            Instance.DestroyGraphics();

            var delay = SidebarAnimationSettings.ContentAppearDelay;
            var delay_between = SidebarAnimationSettings.DelayBetweenCharts;

            LeanTween.cancel(Instance.gameObject);

            LeanTween.delayedCall(Instance.gameObject, delay, () =>
            {
                var instance = Instantiate(Instance.decideOptionsPrefab, Instance.parentOfGraphics);
                instance.Display(humanMonde, cognitiveMode);
                Instance.instanceDecideOptions.Add(instance);
            });
            // Instance.humanButton.onClick.AddListener(humanMonde);
            // Instance.cognitiveButton.onClick.AddListener(cognitiveMode);
            // Debug.Log("OpenDecisionPanel");
            // Instance.OpenDecisionPanelInternal(humanMonde, cognitiveMode);
            // Instance.OpenDecisionPanelInternal();
        }


        private void SetupSidebar(SidebarData sidebar)
        {
            // CloseSidebar();

            backgroundAnimation.ShowRect(SidebarAnimationSettings.BackgroundAppearDuration);
            DestroyGraphics();

            var delay = SidebarAnimationSettings.ContentAppearDelay;
            var delay_between = SidebarAnimationSettings.DelayBetweenCharts;

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
                    if (tableChart.TableType == "product")
                    {
                        ProductTableChart instance = Instantiate(tableChartPrefab, parentOfGraphics);
                        instance.Display(tableChart);
                        instanceTableCharts.Add(instance);
                        responsiveContainer.Resize(true);
                    }
                });
                delay += delay_between;
            });

        }



        private void DestroyGraphics()
        {
            instanceDecideOptions?.ForEach(decideOptions =>
            {
                decideOptions.enabled = false;
                var worldPosition = decideOptions.transform.position;
                decideOptions.transform.SetParent(parentOfGraphicsTemporal, true);
                decideOptions.transform.position = worldPosition;
                Destroy(decideOptions.gameObject, SidebarAnimationSettings.ContentCloseDuration);
            });
            instanceLineCharts?.ForEach(lineChart =>
            {
                lineChart.enabled = false;
                var worldPosition = lineChart.transform.position;
                lineChart.transform.SetParent(parentOfGraphicsTemporal, true);
                lineChart.transform.position = worldPosition;
                Destroy(lineChart.gameObject, SidebarAnimationSettings.ContentCloseDuration);
            });
            instanceBarCharts?.ForEach(BarChart =>
            {
                BarChart.enabled = false;
                var worldPosition = BarChart.transform.position;
                BarChart.transform.SetParent(parentOfGraphicsTemporal, true);
                BarChart.transform.position = worldPosition;
                Destroy(BarChart.gameObject, SidebarAnimationSettings.ContentCloseDuration);
            });
            instanceTableCharts?.ForEach(tableChart =>
            {
                tableChart.enabled = false;
                var worldPosition = tableChart.transform.position;
                tableChart.transform.SetParent(parentOfGraphicsTemporal, true);
                tableChart.transform.position = worldPosition;
                Destroy(tableChart.gameObject, SidebarAnimationSettings.ContentCloseDuration);
            });

            instanceLineCharts = new();
            instanceBarCharts = new();
            instanceTableCharts = new();
            instanceDecideOptions = new();
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