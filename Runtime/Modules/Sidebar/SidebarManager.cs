using System.Collections.Generic;
using UnityEngine;
using Intelmatix.Exoa.Responsive;
using Intelmatix.Base;
using static Intelmatix.Data.QuestionsData;
using Intelmatix;
using Intelmatix.Data;
using Intelmatix.Modules.Sidebar;
using Intelmatix.Templates;
using Intelmatix.Settings;

namespace Intelmatix
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
        [SerializeField] private DecideMode decideModePrefab;
        [SerializeField] private DecideOption decideOptionPrefab;

        private List<StackedLineChart> instanceLineCharts = new List<StackedLineChart>();
        private List<StackedBarchart> instanceBarCharts = new List<StackedBarchart>();
        private List<ProductTableChart> instanceTableCharts = new List<ProductTableChart>();
        private List<DecideMode> instanceDecideModes = new List<DecideMode>();
        private List<DecideOption> instanceDecideOptions = new List<DecideOption>();

        void Start()
        {
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
                var instance = Instantiate(Instance.decideModePrefab, Instance.parentOfGraphics);
                instance.Display(humanMonde, cognitiveMode);
                Instance.instanceDecideModes.Add(instance);
            });
            // Instance.humanButton.onClick.AddListener(humanMonde);
            // Instance.cognitiveButton.onClick.AddListener(cognitiveMode);
            // Debug.Log("OpenDecisionPanel");
            // Instance.OpenDecisionPanelInternal(humanMonde, cognitiveMode);
            // Instance.OpenDecisionPanelInternal();
        }


        private void SetupSidebar(SidebarData sidebar)
        {
            if (sidebar == null)
            {
                Debug.LogWarning("SetupSidebar, sidebar is null");
                return;
            }

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

            sidebar.Decisions.ForEach(decision =>
            {
                LeanTween.delayedCall(this.gameObject, delay, () =>
                {
                    var instance = Instantiate(decideOptionPrefab, parentOfGraphics);
                    instance.Display(decision);
                    instanceDecideOptions.Add(instance);
                    responsiveContainer.Resize(true);
                });
                delay += delay_between;
            });

        }


        private void DestroyGraphics()
        {
            DestroyObjectOfList(instanceLineCharts);
            DestroyObjectOfList(instanceBarCharts);
            DestroyObjectOfList(instanceTableCharts);
            DestroyObjectOfList(instanceDecideModes);
            DestroyObjectOfList(instanceDecideOptions);
        }

        private void DestroyObjectOfList<T>(List<T> list) where T : MonoBehaviour
        {
            list.ForEach(item =>
            {
                item.enabled = false;
                // var worldPosition = item.transform.position;
                item.transform.SetParent(parentOfGraphicsTemporal, true);
                // item.transform.position = worldPosition;
                Destroy(item.gameObject, SidebarAnimationSettings.ContentCloseDuration);
            });
            list.Clear();
        }

    }
}

