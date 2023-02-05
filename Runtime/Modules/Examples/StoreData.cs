using System.Collections.Generic;
using Intelmatix.Structure;
using UnityEngine;

namespace Intelmatix.Data
{
    [System.Serializable]
    public class StoreData : DataReference<StoreData>
    {
        public List<Notification> Notifications { get; set; }


        [System.Serializable]
        public class KPI
        {
            [SerializeField] private int id;
            [SerializeField] private float kpi_value;
            [SerializeField] private string kpi;
            [SerializeField] private string subtitle;
            [SerializeField] private string type;

            public int ID => id;
            public float KPIValue => kpi_value;
            public string Title => kpi;
            public string Subtitle => subtitle;
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

