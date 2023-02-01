using System;
using System.Collections.Generic;
using UnityEngine;
namespace Intelmatix.Modules.Questions.Primitives
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
            public string Title => title;
            public string Path => path;

            public void OpenMap()
            {
                ApplicationManager.Instance.OpenMap(path);
            }
        }
    }
}

