using System;
using System.Collections.Generic;
using UnityEngine;
namespace Intelmatix.Data
{
    [Serializable]
    public class QuestionsData
    {

        [SerializeField] private List<Tab> tabs;
        public List<Tab> Tabs => tabs;

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

            public string Title => title;
            public string Path => path;
            public string Mode => mode;

            public bool IsCognitive => mode.ToLower() == "cognitive";

            public void OpenMap()
            {
            }
        }
    }
}

