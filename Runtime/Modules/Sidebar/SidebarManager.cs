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
using System.Linq;

namespace Intelmatix
{
    public class SidebarManager : Singleton<SidebarManager, SidebarReference>
    {
        private SidebarReference sidebarReference => dataReference;

        [SerializeField] public QuestionsReference questionsReference;

        [Header("References")]
        [SerializeField] private Transform parentOfGraphics;
        [SerializeField] private Transform parentOfGraphicsTemporal;
        [SerializeField] private ResponsiveContainer responsiveContainer;
        [SerializeField] private BakgroundAnim backgroundAnimation;
        [SerializeField] private KPIController kpiController;
        [Space]

        [Header("Graphics")]
        [SerializeField] private StackedLineChart lineChartPrefab;
        [SerializeField] private StackedBarchart barchartPrefab;
        [SerializeField] private ProductTableChart tableChartPrefab;
        [SerializeField] private StadisticsTableChart StadisticsTablePrefab;
        [Space]
        [SerializeField] private DecideMode decideModePrefab;
        [SerializeField] private DecideOption decideOptionPrefab;

        private List<StackedLineChart> listOfLineCharts;
        private List<StackedBarchart> listOfBarcharts;
        private List<ProductTableChart> listOfTableCharts;
        private List<StadisticsTableChart> listOfTableStadisticsCharts;
        private List<DecideMode> listOfDecideModes;
        private List<DecideOption> listOfDecideOptions;


        void Start()
        {
            if (Object.ReferenceEquals(sidebarReference, null)) return;
            listOfLineCharts = new List<StackedLineChart>();
            listOfBarcharts = new List<StackedBarchart>();
            listOfTableCharts = new List<ProductTableChart>();
            listOfTableStadisticsCharts = new List<StadisticsTableChart>();
            listOfDecideModes = new List<DecideMode>();
            listOfDecideOptions = new List<DecideOption>();
        }

        void OnEnable()
        {
            if (Object.ReferenceEquals(sidebarReference, null)) return;
            parentOfGraphics.DestroyChildren();

            questionsReference.OnDataChanged += SetupKPI;

            sidebarReference.OnDataChanged += SetupSidebar;
            // UIManager.OnTabSelected += OnTabSelected;
            UIManager.PreQuestionSelectedEvent += OnQuestionSelected;
            UIManager.PreTabSelectedEvent += OnTabSelectedHandler;
        }

        void OnDisable()
        {
            if (Object.ReferenceEquals(sidebarReference, null)) return;
            questionsReference.OnDataChanged -= SetupKPI;

            sidebarReference.OnDataChanged -= SetupSidebar;
            UIManager.PreQuestionSelectedEvent -= OnQuestionSelected;
            UIManager.PreTabSelectedEvent -= OnTabSelectedHandler;
            // UIManager.OnTabSelected -= OnTabSelected;
        }
        void OnTabSelectedHandler(Tab tab)
        {
            if (tab == null)
            {
                RestoreKPI();
                CloseSidebar();
                return;
            }
            var isCognitiveMode = tab.Questions.Any(q => q.IsCognitive || q.IsHumanMode);
            if (!isCognitiveMode)
            {
                RestoreKPI();
                CloseSidebar();
            }
        }
        void OnQuestionSelected(Question question)
        {
            if (question == null || !question.IsCognitive && !question.IsHumanMode)
            {
                RestoreKPI();
                CloseSidebar();
            }

        }
        private void RestoreKPI()
        {
            Instance?.kpiController.Restore();
        }
        private void SetupKPI(QuestionsData questionsData)
        {
            kpiController.SetKPI(questionsData.KPIs);
        }
        public static void AddKPIDecision(List<SidebarData.KPIDecision> kpis)
        {
            Instance?.kpiController.AddKPIDecision(kpis);
        }
        public static void RemoveKPIDecision(List<SidebarData.KPIDecision> kpis)
        {
            Instance?.kpiController.RemoveKPIDecision(kpis);
        }
        public static void SetKPI(List<StoreData.KPI> kpis)
        {
            Instance?.kpiController.SetKPI(kpis);
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
                instance.Display(humanMonde: () =>
                {
                    Instance.DestroyObjectOfList(Instance.listOfDecideOptions);
                    Instance.RestoreKPI();
                    humanMonde?.Invoke();
                }, cognitiveMode: () =>
                {
                    Instance.DestroyObjectOfList(Instance.listOfDecideOptions);
                    Instance.RestoreKPI();
                    cognitiveMode?.Invoke();
                });
                Instance.listOfDecideModes.Add(instance);
            });
        }


        private void SetupSidebar(SidebarData sidebar)
        {
            LeanTween.cancel(this.gameObject);

            if (sidebar == null)
            {
                Debug.LogWarning("SetupSidebar, sidebar is null");
                return;
            }

            backgroundAnimation.ShowRect(SidebarAnimationSettings.BackgroundAppearDuration);

            var delay = SidebarAnimationSettings.ContentAppearDelay;
            if (sidebar.Decisions.Count > 0)
            {
                DestroyObjectOfList(listOfDecideOptions);

                InstantiateObjectsFromList(sidebar.Decisions, decideOptionPrefab, listOfDecideOptions, ref delay);
            }
            else
            {
                DestroyGraphics();
                InstantiateObjectsFromList(sidebar.LineCharts, lineChartPrefab, listOfLineCharts, ref delay);
                InstantiateObjectsFromList(sidebar.BarCharts, barchartPrefab, listOfBarcharts, ref delay);
                InstantiateObjectsFromList(sidebar.TableCharts, tableChartPrefab, listOfTableCharts, ref delay);
                InstantiateObjectsFromList(sidebar.TableStadistics, StadisticsTablePrefab, listOfTableStadisticsCharts, ref delay);
                InstantiateObjectsFromList(sidebar.Decisions, decideOptionPrefab, listOfDecideOptions, ref delay);
            }

        }


        private void DestroyGraphics()
        {
            DestroyObjectOfList(listOfLineCharts);
            DestroyObjectOfList(listOfBarcharts);
            DestroyObjectOfList(listOfTableCharts);
            DestroyObjectOfList(listOfTableStadisticsCharts);
            DestroyObjectOfList(listOfDecideModes);
            DestroyObjectOfList(listOfDecideOptions);
        }

        private void InstantiateObjectsFromList<T, X>(List<T> list, X prefab, List<X> instantiateList, ref float initialDelay) where X : BaseChart<T>
        {
            var delay = initialDelay;
            list.ForEach(item =>
            {
                LeanTween.delayedCall(this.gameObject, delay, () =>
                {
                    var instance = Instantiate(prefab, parentOfGraphics);
                    instance.transform.SetAsLastSibling();
                    instance.Display(item);
                    instantiateList.Add(instance);
                });
                delay += SidebarAnimationSettings.DelayBetweenCharts;
            });
            initialDelay = delay;
        }

        private void DestroyObjectOfList<T>(List<T> list) where T : MonoBehaviour
        {
            LeanTween.cancel(this.gameObject);
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




// private List<StackedLineChart> instanceLineCharts = new List<StackedLineChart>();
// private List<StackedBarchart> instanceBarCharts = new List<StackedBarchart>();
// private List<ProductTableChart> instanceTableCharts = new List<ProductTableChart>();
// private List<DecideMode> instanceDecideModes = new List<DecideMode>();
// private List<DecideOption> instanceDecideOptions = new List<DecideOption>();



// sidebar.LineCharts.ForEach(lineChart =>
// {
//     LeanTween.delayedCall(this.gameObject, delay, () =>
//     {
//         var instance = Instantiate(lineChartPrefab, parentOfGraphics);
//         instance.Display(lineChart);
//         instanceLineCharts.Add(instance);
//         responsiveContainer.Resize(true);
//     });
//     delay += delay_between;
// });
// sidebar.BarCharts.ForEach(BarChart =>
// {
//     LeanTween.delayedCall(this.gameObject, delay, () =>
//     {
//         StackedBarchart instance = Instantiate(barchartPrefab, parentOfGraphics);
//         instance.Display(BarChart);
//         instanceBarCharts.Add(instance);
//         responsiveContainer.Resize(true);
//     });
//     delay += delay_between;
// });

// sidebar.TableCharts.ForEach(tableChart =>
// {
//     LeanTween.delayedCall(this.gameObject, delay, () =>
//     {
//         if (tableChart.TableType == "product")
//         {
//             ProductTableChart instance = Instantiate(tableChartPrefab, parentOfGraphics);
//             instance.Display(tableChart);
//             instanceTableCharts.Add(instance);
//             responsiveContainer.Resize(true);
//         }
//     });
//     delay += delay_between;
// });

// sidebar.Decisions.ForEach(decision =>
// {
//     LeanTween.delayedCall(this.gameObject, delay, () =>
//     {
//         var instance = Instantiate(decideOptionPrefab, parentOfGraphics);
//         instance.Display(decision);
//         instanceDecideOptions.Add(instance);
//         responsiveContainer.Resize(true);
//     });
//     delay += delay_between;
// });

// private void OnTabSelected(Tab tab)
// {
//     if(tab.)
//     CloseSidebar();
// }


// Instance.humanButton.onClick.AddListener(humanMonde);
// Instance.cognitiveButton.onClick.AddListener(cognitiveMode);
// Debug.Log("OpenDecisionPanel");
// Instance.OpenDecisionPanelInternal(humanMonde, cognitiveMode);
// Instance.OpenDecisionPanelInternal();