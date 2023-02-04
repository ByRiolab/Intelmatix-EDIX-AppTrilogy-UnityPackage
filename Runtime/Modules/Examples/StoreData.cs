using System.Collections.Generic;
using Intelmatix.Structure;
using UnityEngine;

namespace Intelmatix.Data
{
    public class StoreData : DataReference<StoreData>
    {
        public List<KPI> KPIs { get; set; }
        public List<Card> Cards { get; set; }
    }

    public class KPI
    {
        public string Id { get; set; }
        public string Kpi { get; set; }
        public int KpiValue { get; set; }
        public string Subtitle { get; set; }
        public string Type { get; set; }
    }

    public class Label
    {
        public int Id { get; set; }
        public string value { get; set; }
    }

    public class Card
    {
        public string Position { get; set; }
        public List<Label> Labels { get; set; }
    }
}
