using System;
using System.Collections.Generic;
using UnityEngine;
using static Intelmatix.Data.StoreData;

namespace Intelmatix.Data
{
    [Serializable]
    public class QuestionsData
    {

        [SerializeField] private List<Tab> tabs;
        public List<Tab> Tabs => tabs;
        [SerializeField] private List<KPI> kpis;
        public List<KPI> KPIs => kpis;

        [Serializable]
        public class Tab
        {
            [SerializeField] private string title;
            [SerializeField] private string global_question;
            [SerializeField] private List<Question> questions;
            public string Tittle => title;
            public string GlobalQuestion => global_question;
            public List<Question> Questions => questions;

        }
        [Serializable]
        public class Question
        {
            [SerializeField] private string title;
            [SerializeField] private string path;
            [SerializeField] private string mode;
            [SerializeField] private string visualization_type;

            public string Title => title;
            public string Path => path;
            public string Mode => mode;

            public enum VisualizationType
            {
                None,
                Points,
                Arcs,
                Mumaris,
                Decide,
            }
            public VisualizationType Visualization => (VisualizationType)Enum.Parse(typeof(VisualizationType), visualization_type, true);
            public bool IsCognitive => mode.ToLower() == "cognitive";
            public bool IsHumanMode => mode.ToLower().Contains("human");

            public void OpenMap()
            {
            }
        }
    }
}

