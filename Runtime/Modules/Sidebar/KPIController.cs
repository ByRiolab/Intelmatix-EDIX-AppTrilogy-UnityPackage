using System;
using System.Collections;
using System.Collections.Generic;
using Intelmatix.Data;
using UnityEngine;
using static Intelmatix.Data.StoreData;
using TMPro;
using static Intelmatix.Data.SidebarData;

namespace Intelmatix.Templates
{
    public class KPIController : MonoBehaviour
    {
        [System.Serializable]
        private class KPI_Item
        {
            [SerializeField] private TextMeshProUGUI title;
            [SerializeField] private TextMeshProUGUI subtitle;
            [SerializeField] private TextMeshProUGUI value;

            private float initialValue;
            private float currentValue;

            public void SetKPI(KPI kpi)
            {
                title.text = kpi.Title;
                subtitle.text = kpi.Subtitle;
                initialValue = currentValue = kpi.KPIValue;
                value.text = kpi.KPIValue.GetNumberConversion();
                value.color = Color.white;
            }

            public void AddKPIDecision(KPIDecision kpiDecision)
            {
                var newValue = currentValue;
                if (kpiDecision.OperationType == KPIDecision.Operation.increment)
                {
                    newValue += kpiDecision.ExtraValue;
                }
                else if (kpiDecision.OperationType == KPIDecision.Operation.decrement)
                {
                    newValue -= kpiDecision.ExtraValue;
                }

                Color targetColor = Color.white;
                if (newValue > initialValue)
                {
                    targetColor = Color.green;
                }
                else if (newValue < initialValue)
                {
                    targetColor = Color.red;
                }

                LeanTween.cancel(value.gameObject);
                LeanTween.value(value.gameObject, value.color, targetColor, 0.5f).setOnUpdate((Color color) =>
                {
                    value.color = color;
                });

                LeanTween.value(value.gameObject, currentValue, newValue, 0.5f).setOnUpdate((float value) =>
                {
                    this.value.text = value.GetNumberConversion();
                });
                currentValue = newValue;
            }

            public void RemoveKPIDecision(KPIDecision kpiDecision)
            {
                var newValue = currentValue;
                if (kpiDecision.OperationType == KPIDecision.Operation.increment)
                {
                    newValue -= kpiDecision.ExtraValue;
                }
                else if (kpiDecision.OperationType == KPIDecision.Operation.decrement)
                {
                    newValue += kpiDecision.ExtraValue;
                }

                Color targetColor = Color.white;
                if (newValue > initialValue)
                {
                    targetColor = Color.green;
                }
                else if (newValue < initialValue)
                {
                    targetColor = Color.red;
                }

                LeanTween.cancel(value.gameObject);
                LeanTween.value(value.gameObject, value.color, targetColor, 0.5f).setOnUpdate((Color color) =>
                {
                    value.color = color;
                });
                LeanTween.value(value.gameObject, currentValue, newValue, 0.5f).setOnUpdate((float value) =>
                {
                    this.value.text = value.GetNumberConversion();
                });
                currentValue = newValue;

            }

        }
        private List<KPI> kpis = new List<KPI>();

        [SerializeField] private KPI_Item kpi1;
        [SerializeField] private KPI_Item kpi2;
        [SerializeField] private KPI_Item kpi3;

        [Header("Settings")]
        [SerializeField] private Color increaseColor = Color.green;
        [SerializeField] private Color decreaseColor = Color.red;
        [SerializeField] private Color neutralColor = Color.white;

        internal void AddKPIDecision(List<SidebarData.KPIDecision> kpis)
        {
            if (kpis.Count == 0)
            {
                return;
            }

            kpi1.AddKPIDecision(kpis[0]);
            kpi2.AddKPIDecision(kpis[1]);
            kpi3.AddKPIDecision(kpis[2]);
        }
        internal void RemoveKPIDecision(List<SidebarData.KPIDecision> kpis)
        {
            if (kpis.Count == 0)
            {
                return;
            }

            kpi1.RemoveKPIDecision(kpis[0]);
            kpi2.RemoveKPIDecision(kpis[1]);
            kpi3.RemoveKPIDecision(kpis[2]);
        }

        internal void SetKPI(List<KPI> kpis)
        {
            if (kpis.Count == 0)
            {
                return;
            }
            this.kpis = kpis;

            kpi1.SetKPI(kpis[0]);
            kpi2.SetKPI(kpis[1]);
            kpi3.SetKPI(kpis[2]);
        }

        public void Restore()
        {

            kpi1.SetKPI(kpis[0]);
            kpi2.SetKPI(kpis[1]);
            kpi3.SetKPI(kpis[2]);
        }
    }
}