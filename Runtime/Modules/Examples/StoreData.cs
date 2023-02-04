using System.Collections.Generic;
using Intelmatix.Structure;
using UnityEngine;

namespace Intelmatix.Data
{
    [System.Serializable]
    public class StoreData : DataReference<StoreData>
    {
        public List<KPI> KPIs { get; set; }
        public List<Notification> Cards { get; set; }

        [System.Serializable]
        public class KPI
        {
            public string Id { get; set; }
            public string Kpi { get; set; }
            public int KpiValue { get; set; }
            public string Subtitle { get; set; }
            public string Type { get; set; }
        }

        [System.Serializable]
        public class Card
        {
            [SerializeField] private int id;
            [SerializeField] private string label;
            [SerializeField] private string icon;

            public int ID => id;
            public string Label => label;
            public string Icon => icon;

            public Card(int id, string label, string icon)
            {
                this.id = id;
                this.label = label;
                this.icon = icon;
            }
        }

        [System.Serializable]
        public class Notification
        {
            [SerializeField] private string position;
            [SerializeField] private List<Card> cards;

            public string Position => position;
            public List<Card> Cards => cards;

            public Notification(string position, List<Card> cards)
            {
                this.position = position;
                this.cards = cards;
            }
        }
    }
}

